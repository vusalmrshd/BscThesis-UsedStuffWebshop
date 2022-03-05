using System;
using System.Collections.Generic;
using Models.Database.Sps;

namespace Models.Response.Auth
{
    public class LoginResponseModel
    {
        public long userId { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public bool isSeller { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public List<SpPermissionResponse> permissions { get; set; }
        public string token { get; set; }
        public DateTimeOffset createdAt { get; set; }

    }
}
