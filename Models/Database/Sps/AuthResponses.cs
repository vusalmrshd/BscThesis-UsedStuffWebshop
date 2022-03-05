using System;
namespace Models.Database.Sps
{
    public class SpLoginResponse
    {
        public long userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public bool isSeller { get; set; }
        public string roleName { get; set; }
    }

    public class SpPermissionResponse
    {
        public long permissionId { get; set; }
        public string permissionName { get; set; }
    }
}
