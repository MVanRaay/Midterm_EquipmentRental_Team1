using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.Net.Http.Headers;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IHttpClientFactory _http;

        public EquipmentController(IHttpClientFactory http)
        {
            _http = http;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("https://localhost:7088/api/equipment/available");
            if (response.IsSuccessStatusCode)
            {
                var equipment = response.Content.ReadFromJsonAsync<List<Equipment>>().Result;

                if (User.IsInRole("Admin"))
                {
                    return View("AdminDashboard", equipment);
                }
                else
                {
                    return View("UserDashboard", equipment);
                }
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }
    }
}
