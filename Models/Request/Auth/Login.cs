using System;
namespace Models.Request.Auth
{
    public class Login
    {
        public string userNameOrEmail { get; set; }
        public string password { get; set; }
    }
}
