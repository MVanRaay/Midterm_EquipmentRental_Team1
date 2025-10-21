using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = httpClientFactory.CreateClient();
            var loginRequest = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password,
            };

            var response = await client.PostAsJsonAsync("https://localhost:7191/api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var result = JsonSerializer.Deserialize<JsonElement>(token, options);

                model.Token = result.GetProperty("token").GetString();
                HttpContext.Session.SetString("JWToken", model.Token);

                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                return RedirectToAction("Index", "Equipment");
            }
            else
            {
                model.ErrorMessage = "Invalid username or password";
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login", "Auth");
        }
    }
}
