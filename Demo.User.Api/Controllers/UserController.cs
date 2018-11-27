using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.User.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {

        [Route("User")]
        public IActionResult Index()
        {
            return new JsonResult(new { Name = "demo", Success = true });
        }

        [AllowAnonymous]
        [Route("User/SignIn")]
        public IActionResult SignIn()
        {
            return new JsonResult(new { Message = "请登录" });
        }
    }
}
