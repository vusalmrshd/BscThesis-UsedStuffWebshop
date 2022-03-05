using System;
using System.Threading.Tasks;
using Models.Request.Order;
using Models.Response.CoreResponse;

namespace Core.Interface
{
    public interface IStripeService
    {
        
        Task<CoreResponseModel> createCharge(PlaceOrderRequest order,decimal bill, string customerName, string sellerId);
    }
}
