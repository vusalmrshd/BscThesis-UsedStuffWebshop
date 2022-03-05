using System;
using Microsoft.AspNetCore.Http;

namespace Models.Request.Auth
{
    public class Register
    {
        public long userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string status { get; set; }
        public string userName { get; set; }
        public bool isSeller { get; set; }
        public IFormFile image { get; set; }
        public long createdBy { get; set; }
        public string link { get; set; }
    }
}
