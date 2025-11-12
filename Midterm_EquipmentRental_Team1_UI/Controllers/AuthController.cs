using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewData.Add("Title", "Login");
            var model = new LoginViewModel();
            return View("Login", model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _http.CreateClient();
            var loginRequest = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password,
            };

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var result = JsonSerializer.Deserialize<JsonElement>(token, options);
                model.Token = result.GetProperty("token").GetString();

                var handler = new JwtSecurityTokenHandler();
                var cookieToken = handler.ReadJwtToken(model.Token);

                var expClaim = cookieToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!;
                var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;

                var claims = cookieToken.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                {
                    ExpiresUtc = expTime,
                    IsPersistent = true,
                    AllowRefresh = true,
                });
                HttpContext.Session.SetString("JWToken", model.Token!);

                var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.ToString();

                if (role == "Admin")
                {
                    return RedirectToAction("AdminDashboard", "Customer");
                }
                else
                {
                    return RedirectToAction("UserDashboard", "Customer");
                }
            }
            else
            {
                ViewData.Add("Title", "Login");
                model.ErrorMessage = "Invalid username or password";
                return View(model);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWToken");
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            var model = new ErrorViewModel { RequestId = "Access Denied" };
            return View("Error", model);
        }
    }
}
