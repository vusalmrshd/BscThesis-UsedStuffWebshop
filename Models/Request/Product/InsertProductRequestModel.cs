using System;
using Microsoft.AspNetCore.Http;

namespace Models.Request.Product
{
    public class InsertProductRequestModel
    {
        public long productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public IFormFile image { get; set; }
        public long sellerId { get; set; }
        public bool isDiscount { get; set; }
        public decimal discountPrice { get; set; }
        public string comment { get; set; }
        public string condition { get; set; }
        public string location { get; set; }
        public long catId { get; set; }

    }
}
