using System;
namespace Models.Request.Auth
{
    public class UpdatePassword
    {
        public string email { get; set; }
        public string password { get; set; }
        public string code { get; set; }
    }
}
