using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Database.Entity
{
    public class UserForgetPassword
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string email { get; set; }
        public string code { get; set; }

    }
}
