using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
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
            var model = new LoginViewModel();
            return View("Login", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _http.CreateClient();
            var loginRequest = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password,
            };

            var response = await client.PostAsJsonAsync("https://localhost:7088/api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var result = JsonSerializer.Deserialize<JsonElement>(token, options);
                model.Token = result.GetProperty("token").GetString();
                HttpContext.Session.SetString("JWToken", model.Token!);

                var handler = new JwtSecurityTokenHandler();
                var cookieToken = handler.ReadJwtToken(model.Token);

                var claims = cookieToken.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToAction("Dashboard", "Equipment");
            }
            else
            {
                model.ErrorMessage = "Invalid username or password";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWToken");
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            var model = new ErrorViewModel { RequestId = "Access Denied" };
            return View("Error", model);
        }
    }
}
