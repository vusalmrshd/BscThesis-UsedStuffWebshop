using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Database.Context;
using Models.Database.Sps;
using Models.Request.Auth;

namespace Core.Services
{
    public class Procedures : IProcedures
    {
        private DatabaseContext _context;
        private ICommonService _common;

        public Procedures(DatabaseContext _context, ICommonService _common)
        {
            this._context = _context;
            this._common = _common;
        }

        public async Task<SpLoginResponse> spLoginUser(Login login)
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    var userNameOrEmail = new SqlParameter("userNameOrEmail", login.userNameOrEmail);
                    var password = new SqlParameter("password", _common.passwordHasher(login.password));
                    var user = await _context.Set<SpLoginResponse>()
                    .FromSqlInterpolated($"EXEC sp_login {userNameOrEmail},{password}")
                    .ToListAsync()
                    .ConfigureAwait(false);

                    return user[0];
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<SpPermissionResponse>> spGetUserPermissions(long userId)
        {
            try
            {
                using (var cmd = new SqlCommand())
                {
                    var id = new SqlParameter("userId", userId);
                    var permissions = await _context.Set<SpPermissionResponse>()
                    .FromSqlInterpolated($"EXEC sp_getUserPermissions {id}")
                    .ToListAsync()
                    .ConfigureAwait(false);

                    return permissions;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
