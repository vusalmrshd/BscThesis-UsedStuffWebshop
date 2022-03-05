using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Database.Entity
{
    public class User : EntityPersistent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public bool isSeller { get; set; }
        public string profileImage { get; set; }

    }
}
