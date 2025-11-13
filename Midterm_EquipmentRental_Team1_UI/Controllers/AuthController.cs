using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Global;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _http;

        public AuthController(IHttpClientFactory http)
        {
            _http = http;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData.Add("Title", "Login");
            return View("Login");
        }

        [HttpGet]
        public IActionResult DoLogin(string returnUrl = "/")
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                },
                "oidc"
            );
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Auth", "Login")
                },
                CookieAuthenticationDefaults.AuthenticationScheme
            );
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    var client = _http.CreateClient();

        //    var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/auth/login");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var token = await response.Content.ReadAsStringAsync();
        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true,
        //        };

        //        var result = JsonSerializer.Deserialize<JsonElement>(token, options);
        //        model.Token = result.GetProperty("token").GetString();

        //        var handler = new JwtSecurityTokenHandler();
        //        var cookieToken = handler.ReadJwtToken(model.Token);

        //        var expClaim = cookieToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!;
        //        var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;

        //        var claims = cookieToken.Claims.ToList();
        //        var identity = new ClaimsIdentity(claims, "Cookies");
        //        var principal = new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
        //        {
        //            ExpiresUtc = expTime,
        //            IsPersistent = true,
        //            AllowRefresh = true,
        //        });
        //        HttpContext.Session.SetString("JWToken", model.Token!);

        //        var role = cookieToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

        //        if (role == "Admin")
        //        {
        //            return RedirectToAction("AdminDashboard", "Customer");
        //        }
        //        else
        //        {
        //            return RedirectToAction("UserDashboard", "Customer");
        //        }
        //    }
        //    else
        //    {
        //        ViewData.Add("Title", "Login");
        //        model.ErrorMessage = "Invalid username or password";
        //        return View(model);
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Logout()
        //{
        //    HttpContext.Session.Remove("JWToken");
        //    await HttpContext.SignOutAsync("Cookies");
        //    return RedirectToAction("Login");
        //}

        [HttpGet]
        public IActionResult AccessDenied()
        {
            var model = new ErrorViewModel { RequestId = "Access Denied" };
            return View("Error", model);
        }
    }
}
