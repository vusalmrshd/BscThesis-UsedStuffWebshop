using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Database.Entity
{
    public class Product : EntityPersistent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public bool isDiscounted { get; set; }
        public decimal discountPrice { get; set; }
        public long sellerId { get; set; }
        public string comment { get; set; }
        public string condition { get; set; }
        public string location { get; set; }
        public long categoryId { get; set; }
    }
}
