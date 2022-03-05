using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Database.Context;
using Models.Database.Entity;
using Models.Request.Order;
using Models.Response.CoreResponse;
using Models.Utils;
using Stripe;

namespace Core.Services
{
    public class OrderService : IOrderService
    {
        private ICoreResponseModel _response;
        private IConfiguration _config;
        private DatabaseContext _context;
        private ICommonService _common;
        private IEmailService _emailService;
        private IProcedures _sps;
        private IStripeService _stripe;


        public OrderService(ICoreResponseModel _response,IStripeService _stripe,IConfiguration _config, DatabaseContext _context, ICommonService _common, IProcedures _sps, IEmailService _emailService)
        {
            this._response = _response;
            this._config = _config;
            this._context = _context;
            this._common = _common;
            this._emailService = _emailService;
            this._sps = _sps;
            this._stripe = _stripe;
        }



        public async Task<CoreResponseModel> placeOrder(PlaceOrderRequest request)
        {
            try
            {
              
                List<long> sellers = new List<long>();
                foreach(var seller in request.orderProducts)
                {
                    if (sellers.Count < 1) sellers.Add(seller.sellerId);
                    else
                    {
                        if (!sellers.Contains(seller.sellerId))
                        {
                            sellers.Add(seller.sellerId);
                        }
                    }
                }
                foreach(var seller in sellers)
                {
                    decimal totalBill = 0;
                    foreach(var product in request.orderProducts)
                    {
                        if(seller == product.sellerId)
                        {
                            var bill = product.quantity * product.price;
                            totalBill = totalBill + bill;
                        }
                    }
                    var response = await _stripe.createCharge(request, totalBill, request.customerName, seller + "");
                    if (response.success)
                    {
                        var order = new Models.Database.Entity.Order()
                        {
                            createdAt = DateTimeOffset.UtcNow,
                            updatedAt = DateTimeOffset.UtcNow,
                            createdBy = request.customerId,
                            updatedBy = request.customerId,
                            address = request.customerAddress,
                            sellerId = seller,
                            customerEmail = request.customerEmail,
                            customerName = request.customerEmail,
                            customerId = request.customerId,
                            isActive = true,
                            orderPrice = totalBill
                        };

                        await _context.orders.AddAsync(order).ConfigureAwait(false);
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                        foreach (var prodcut in request.orderProducts)
                        {
                            if (prodcut.sellerId == seller)
                            {
                                await _context.orderProducts.AddAsync(new Models.Database.Entity.OrderProducts()
                                {
                                    orderId = order.orderId,
                                    price = prodcut.price,
                                    productId = prodcut.productId,
                                    quantity = prodcut.quantity
                                });
                            }

                        }
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                    }
                   
                }
                return _response.getSuccessResponse(_MESSAGES.orderPlaced, null);

            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> getCustomerOrders(long customerId)
        {
            try
            {
                var orders = await _context.orders
                    .Where(x => x.customerId == customerId || x.sellerId == customerId)
                    .Select(x => new
                    {
                        orderId = x.orderId,
                        orderPrice = x.orderPrice,
                        address = x.address,
                        createdAt = x.createdAt,
                        customerName = _context.users.Where(y => y.userId == x.customerId).Select(y => y.name).SingleOrDefault(),
                        customerEmail = x.customerEmail,
                        sellerName=_context.users.Where(y => y.userId == x.sellerId).Select(y=>y.name).SingleOrDefault(),
                        sellerEmail = _context.users.Where(y => y.userId == x.sellerId).Select(y => y.email).SingleOrDefault(),
                        sellerId = x.sellerId,
                        products = _context.orderProducts.Where(op => op.orderId == x.orderId).Select(op => new
                        {
                            productId = op.productId,
                            name = _context.products.Where(p => p.productId == op.productId).Select(p => p.name).SingleOrDefault(),
                            price = op.price,
                            quantity = op.quantity
                        }).ToList()
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return _response.getSuccessResponse(_MESSAGES.success, orders);

            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> deleteOrder(long orderId)
        {
            try
            {
                var order = await _context.orders.FirstOrDefaultAsync(x => x.orderId == orderId).ConfigureAwait(false);
                var orderProducts = await _context.orderProducts.Where(x => x.orderId == orderId).ToListAsync().ConfigureAwait(false);

                _context.orders.Remove(order);
                _context.orderProducts.RemoveRange(orderProducts);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.recordDeleted, null);
            }
            catch(Exception e)
            {
                return _response.getFailResponse(e.Message, null);
            }
        }

        public async Task<CoreResponseModel> getSellerOrders(long sellerId)
        {
            try
            {
                var orders = await _context.orders
                    .Where(x => x.customerId == sellerId || x.sellerId == sellerId)
                    .Select(x => new
                    {
                        orderId = x.orderId,
                        orderPrice = x.orderPrice,
                        address = x.address,
                        createdAt = x.createdAt,
                        customerName = _context.users.Where(y => y.userId == x.customerId).Select(y => y.name).SingleOrDefault(),
                        customerEmail = x.customerEmail,
                        sellerName = _context.users.Where(y => y.userId == x.sellerId).Select(y => y.name).SingleOrDefault(),
                        sellerEmail = _context.users.Where(y => y.userId == x.sellerId).Select(y => y.email).SingleOrDefault(),
                        sellerId = x.sellerId,
                        products = _context.orderProducts.Where(op=>op.orderId == x.orderId).Select(op=>new
                        {
                            productId = op.productId,
                            name = _context.products.Where(p => p.productId == op.productId).Select(p=>p.name).SingleOrDefault(),
                            price = op.price,
                            quantity = op.quantity
                        }).ToList()
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return _response.getSuccessResponse(_MESSAGES.success, orders);
                
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
    }
}
