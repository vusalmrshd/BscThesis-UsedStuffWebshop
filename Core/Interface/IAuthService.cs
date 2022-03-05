using System;
using System.Threading.Tasks;
using Models.Request.Auth;
using Models.Response.CoreResponse;

namespace Core.Interface
{
    public interface IAuthService
    {
        Task<CoreResponseModel> register(Register register);
        Task<CoreResponseModel> login(Login login);
        Task<CoreResponseModel> updateUser(Register register);
        Task<CoreResponseModel> forgetPassword(ForgetPassowrd forget);
        Task<CoreResponseModel> updatePassword(UpdatePassword update);
        Task<CoreResponseModel> validatePasswordLink(ForgetPassowrd update);
        Task<CoreResponseModel> activateAccount(string email);
    }
}
