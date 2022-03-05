using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Database.Entity
{
    public class Order : EntityPersistent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long orderId { get; set; }
        public string address { get; set; }
        public decimal orderPrice { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public long customerId { get; set; }
        public long sellerId { get; set; }
    }
}
