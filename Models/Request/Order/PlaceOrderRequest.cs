using System;
using System.Collections.Generic;

namespace Models.Request.Order
{
    public class PlaceOrderRequest
    {
        public long sellerId { get; set; }
        public long customerId { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string cardNumber { get; set; }
        public int expMonth { get; set; }
        public int expYear { get; set; }
        public int cvc { get; set; }
        public decimal orderPrice { get; set; }
        public List<OrderProducts> orderProducts { get; set; }
    }

    public class OrderProducts
    {
        public long productId { get; set; }
        public decimal price { get; set; }
        public long quantity { get; set; }
        public long sellerId { get; set; }
    }
}
