using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Database.Sps;
using Models.Request.Auth;

namespace Core.Interface
{
    public interface IProcedures
    {
        Task<SpLoginResponse> spLoginUser(Login login);
        Task<List<SpPermissionResponse>> spGetUserPermissions(long userId);
    }
}
