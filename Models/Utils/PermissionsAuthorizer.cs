using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Database.Context;
using Models.Response.CoreResponse;

namespace Models.Utils
{
    public class PermissionsAuthorizer
    {
        private readonly RequestDelegate _next;

        public PermissionsAuthorizer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, DatabaseContext _context,ICoreResponseModel _response)
        {
            var path = context.Request.Path.Value.ToString();
            //if(path == "/api/v1/auth/login")
            //{
            //    var user = await _context.Users.ToListAsync().ConfigureAwait(false);
            //    context.Response.StatusCode = 200;
            //    await context.Response.WriteAsJsonAsync(_response.getNotAllowedResponse());
            //}
            //else
                await _next(context);
            
            
        }
    }
}
