using System;
namespace Models.Request.Auth
{
    public class ForgetPassowrd
    {
        public string email { get; set; }
        public string link { get; set; }
        public string token { get; set; }
    }
}
