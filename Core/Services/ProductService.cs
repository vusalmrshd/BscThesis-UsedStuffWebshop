using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Database.Context;
using Models.Request.Product;
using Models.Response.CoreResponse;
using Models.Response.Product;
using Models.Utils;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private ICoreResponseModel _response;
        private IConfiguration _config;
        private DatabaseContext _context;
        private ICommonService _common;
        private IEmailService _emailService;
        private IProcedures _sps;


        public ProductService(ICoreResponseModel _response, IConfiguration _config, DatabaseContext _context, ICommonService _common, IProcedures _sps, IEmailService _emailService)
        {
            this._response = _response;
            this._config = _config;
            this._context = _context;
            this._common = _common;
            this._emailService = _emailService;
            this._sps = _sps;
        }


        public async Task<CoreResponseModel> addProduct(InsertProductRequestModel request)
        {
            try
            {
                var imageResult = await _common.saveImageToServer(request.image).ConfigureAwait(false);
                if (!imageResult.success) return imageResult;
                await _context.products.AddAsync(new Models.Database.Entity.Product()
                {
                    createdAt = DateTimeOffset.UtcNow,
                    updatedAt = DateTimeOffset.UtcNow,
                    createdBy = request.sellerId,
                    updatedBy = request.sellerId,
                    sellerId  = request.sellerId,
                    isActive  = true,
                    comment =request.comment,
                    condition=request.condition,
                    location = request.location,
                    name = request.name,
                    price = request.price,
                    discountPrice=0,
                    isDiscounted=false,
                    image = $"{imageResult.data}",
                    categoryId=request.catId
                });
                await _context.SaveChangesAsync().ConfigureAwait(false);
                var products = await _context.products.Where(x => x.sellerId == request.sellerId).ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
        public async Task<CoreResponseModel> updateProduct(InsertProductRequestModel request)
        {
            try
            {
                var product = await _context.products.FirstOrDefaultAsync(x => x.productId == request.productId).ConfigureAwait(false);
                if (product == null) return _response.getFailResponse(_MESSAGES.noRecordExist, null);

                string imageLink = "";
                if(request.image != null)
                {
                    var result = await _common.saveImageToServer(request.image).ConfigureAwait(false);
                    if (!result.success) return result;
                    imageLink = $"{result.data}";
                }
                product.name = (request.name == null || request.name == "") ? product.name : request.name;
                product.price = (request.price == 0) ? product.price : (decimal)request.price;
                product.image = (imageLink == null || imageLink == "") ? product.image : imageLink;
                product.updatedAt = DateTimeOffset.UtcNow;
                product.comment = (request.comment == null || request.comment == "") ? product.comment : request.comment;
                product.condition = (request.condition == null || request.condition == "") ? product.condition : request.condition;
                product.location = (request.location == null || request.location == "") ? product.location : request.location;
                product.categoryId = (request.catId == 0) ? product.categoryId : request.catId;
                if (request.isDiscount)
                {
                    product.isDiscounted = true;    
                    product.discountPrice = request.discountPrice;
                }
                else
                {
                    product.isDiscounted = false;
                    product.discountPrice = 0;
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                var products = await _context.products.Where(x => x.sellerId == request.sellerId).ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
        public async Task<CoreResponseModel> deleteProduct(long productId)
        {
            try
            {
                var product = await _context.products.FirstOrDefaultAsync(x => x.productId == productId).ConfigureAwait(false);
                _context.products.Remove(product);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                var products = await _context.products.Where(x => x.sellerId == product.sellerId).ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
        public async Task<CoreResponseModel> getProductsOfSeller(long sellerId)
        {
            try
            {
                var products = await _context.products
                    .Where(x => x.sellerId == sellerId)
                     .Select(x => new ProductsResponseModel()
                     {
                         productId = x.productId,
                         name = x.name,
                         price = x.price,
                         categoryId = x.categoryId,
                         categoryName = _context.categories.Where(c => c.id == x.categoryId).Select(c => c.name).SingleOrDefault(),
                         sellerId = x.sellerId,
                         comment = x.comment,
                         condition = x.condition,
                         discountPrice = x.discountPrice,
                         image = x.image,
                         isDiscounted = x.isDiscounted,
                         location = x.location
                     })
                    .ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
        public async Task<CoreResponseModel> getAllProducts(bool isDiscounted)
        {
            try
            {
                var products = await _context.products
                    .Where(x=>x.isDiscounted == isDiscounted)
                    .Select(x=>new ProductsResponseModel()
                    {
                        productId = x.productId,
                        name = x.name,
                        price =x.price,
                        categoryId=x.categoryId,
                        categoryName = _context.categories.Where(c=>c.id == x.categoryId).Select(c=>c.name).SingleOrDefault(),
                        sellerId=x.sellerId,
                        comment=x.comment,
                        condition=x.condition,
                        discountPrice=x.discountPrice,
                        image=x.image,
                        isDiscounted=x.isDiscounted,
                        location=x.location
                        
                    })
                    .ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> getProductByCat(bool isDiscount, long catId)
        {
            try
            {
                var products = await _context.products
                    .Where(x => x.categoryId == catId && x.isDiscounted == isDiscount)
                    .Select(x => new ProductsResponseModel()
                    {
                        productId = x.productId,
                        name = x.name,
                        price = x.price,
                        categoryId = x.categoryId,
                        categoryName = _context.categories.Where(c => c.id == x.categoryId).Select(c => c.name).SingleOrDefault(),
                        sellerId = x.sellerId,
                        comment = x.comment,
                        condition = x.condition,
                        discountPrice = x.discountPrice,
                        image = x.image,
                        isDiscounted = x.isDiscounted,
                        location = x.location

                    })
                    .ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, products);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> insertCategory(InsertCategory request)
        {
            try
            {
                await _context.categories.AddAsync(new Models.Database.Entity.Category()
                {
                    name = request.name
                }).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, null);
            }
            catch(Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }
        public async Task<CoreResponseModel> getCategories()
        {
            try
            {
                var cats = await _context.categories.ToListAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, cats);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }

        public async Task<CoreResponseModel> deleteCategory(long id)
        {
            try
            {
                var cat = await _context.categories.FirstOrDefaultAsync(x=>x.id == id).ConfigureAwait(false);
                _context.categories.Remove(cat);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return _response.getSuccessResponse(_MESSAGES.success, null);
            }
            catch (Exception ex)
            {
                return _response.getFailResponse(ex.Message, null);
            }
        }


    }
}
