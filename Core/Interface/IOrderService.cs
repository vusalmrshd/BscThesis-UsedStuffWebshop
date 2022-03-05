using System;
using System.Threading.Tasks;
using Models.Request.Order;
using Models.Response.CoreResponse;

namespace Core.Interface
{
    public interface IOrderService
    {
        Task<CoreResponseModel> placeOrder(PlaceOrderRequest request);
        Task<CoreResponseModel> getCustomerOrders(long customerId);
        Task<CoreResponseModel> getSellerOrders(long sellerId);
        Task<CoreResponseModel> deleteOrder(long orderId);
    }
}
