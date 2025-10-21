using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.Text.Json;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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
                HttpContext.Session.SetString("JWToken", model.Token);
                return RedirectToAction("Index", "Product");
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
