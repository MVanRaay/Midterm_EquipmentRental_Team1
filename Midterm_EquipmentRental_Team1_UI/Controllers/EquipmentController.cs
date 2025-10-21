using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EquipmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Index()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = client.GetAsync("https://localhost:7088/api/equipment/available");
            if (response.Result.IsSuccessStatusCode)
            {
                var equipment = response.Result.Content.ReadFromJsonAsync<IEnumerable<Equipment>>().Result;
                return View("UserDasboard", equipment);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
