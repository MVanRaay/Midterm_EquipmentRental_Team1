using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Global;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;

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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var client = _http.CreateClient();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment");
            List<Equipment> equipment;
            EquipmentListViewModel model;

            if (response.IsSuccessStatusCode)
            {
                equipment = response.Content.ReadFromJsonAsync<List<Equipment>>().Result!;
                model = new EquipmentListViewModel { Equipment = equipment };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                model = new EquipmentListViewModel { Equipment = [] };
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }

            ViewData["Title"] = "Equipment";

            if (role == "Admin")
            {
                return View("AdminEquipment", model);
            }
            else
            {
                return View("UserEquipment", model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var client = _http.CreateClient();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment/{id}");
            Equipment equipment;
            EquipmentViewModel model;

            if (response.IsSuccessStatusCode)
            {
                equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                model = new EquipmentViewModel { Equipment = equipment };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                model = new EquipmentViewModel { Equipment = null };
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }

            ViewData["Title"] = "Equipment Details";

            if (role == "Admin")
            {
                return View("AdminEquipmentDetails", model);
            }
            else
            {
                return View("UserEquipmentDetails", model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Equipment";
            var model = new EquipmentViewModel();
            return View("Add", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", model);
            }

            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/equipment/add", model.Equipment);

            if (response.IsSuccessStatusCode)
            {
                var equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                return RedirectToAction("Details", new { id = equipment.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = "Unable to create new Equipment" };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Title"] = "Edit Equipment";
                var equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                var model = new EquipmentViewModel { Equipment = equipment };
                return View("Edit", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] EquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PutAsJsonAsync($"{GlobalUsings.API_BASE_URL}/equipment/{id}", model.Equipment);

            if (response.IsSuccessStatusCode)
            {
                var equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                return RedirectToAction("Details", new { id = equipment.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.DeleteAsync($"{GlobalUsings.API_BASE_URL}/equipment/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Successfully Deleted Equipment";
                return RedirectToAction("GetAll");
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }
    }
}
