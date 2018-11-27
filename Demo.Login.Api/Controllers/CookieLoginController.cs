using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Login.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class CookieLoginController : Controller
    {
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ResponseDTO> Login()
        {
            const string Issuer = "https://gov.uk";

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Role,"Admin",ClaimValueTypes.String,Issuer),
                new Claim(ClaimTypes.Name, "Andrew", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Surname, "Lock", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Country, "UK", ClaimValueTypes.String, Issuer),
                new Claim("ChildhoodHero", "Ronnie James Dio", ClaimValueTypes.String)
            };
            var userIdentity = new ClaimsIdentity(claims, "Passport");

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = true,
                AllowRefresh = false,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
            return new ResponseDTO();
        }

        [AllowAnonymous]
        [Route("User/Deny")]
        public ResponseDTO Deny()
        {
            return new ResponseDTO { Message = "Access Denied." };
        }

        [AllowAnonymous]
        [Route("User/SignIn")]
        public ResponseDTO SignIn()
        {
            return new ResponseDTO { Message = "请登录" };
        }

        [Route("User/Logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("User/SignIn");
        }
    }
}
