using System.Collections.Generic;
using System.Threading.Tasks;
using Ecom.Controllers.APi;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Order;

namespace Ecom.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrderController : BaseController
    {
       [HttpPost("placeOrder")]
       public async Task<IActionResult> placeOrder([FromBody]PlaceOrderRequest request)
       {
            return Ok(await _OrderService.placeOrder(request));
       }

       [HttpGet("getSellerOrders")]
       public async Task<IActionResult> getSellerOrders(long userId)
       {
           return Ok(await _OrderService.getSellerOrders(userId));
       }

       [HttpGet("getCustomerOrders")]
       public async Task<IActionResult> getCustomerOrders(long userId)
       {
           return Ok(await _OrderService.getCustomerOrders(userId));
       }

        [HttpGet("deleteOrder")]
        public async Task<IActionResult> deleteOrder(long orderId)
        {
            return Ok(await _OrderService.deleteOrder(orderId));
        }
    }
}
