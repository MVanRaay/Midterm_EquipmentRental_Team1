using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_UI.Models;


namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View("Login");
        }

        [HttpGet]
        public IActionResult DoLogin(string returnUrl = "/Customer/Dashboard")
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                },
                OpenIdConnectDefaults.AuthenticationScheme
            );
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Login", "Auth")
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            );
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            var model = new ErrorViewModel { RequestId = "Access Denied" };
            return View("Error", model);
        }
    }
}
