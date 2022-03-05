using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Auth;

namespace Ecom.Controllers.APi
{
    [Route("api/v1/[controller]")]
    public class AuthController : BaseController
    {
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> register([FromForm] Register register)
        {
            return Ok(await _AuthService.register(register));
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> login([FromBody] Login login)
        {
            return Ok(await _AuthService.login(login));
        }

        [Route("updateUser")]
        [HttpPost]
        public async Task<IActionResult> updateUser([FromForm] Register update)
        {
            return Ok(await _AuthService.updateUser(update));
        }

        [Route("forgetPassword")]
        [HttpPost]
        public async Task<IActionResult> forgetPassword([FromBody] ForgetPassowrd forget)
        {
            return Ok(await _AuthService.forgetPassword(forget));
        }
        [Route("validatePasswordLink")]
        [HttpPost]
        public async Task<IActionResult> validatePasswordLink([FromBody] ForgetPassowrd forget)
        {
            return Ok(await _AuthService.validatePasswordLink(forget));
        }

        [Route("activateAccount")]
        [HttpGet]
        public async Task<IActionResult> activateAccount([FromQuery]string email)
        {
            return Ok(await _AuthService.activateAccount(email));
        }

        [Route("updatePassword")]
        [HttpPost]
        public async Task<IActionResult> updatePassword([FromBody] UpdatePassword update)
        {
            return Ok(await _AuthService.updatePassword(update));
        }
    }
}
