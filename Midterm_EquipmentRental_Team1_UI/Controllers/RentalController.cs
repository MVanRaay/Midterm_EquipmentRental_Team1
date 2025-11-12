using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_UI.Global;
using Midterm_EquipmentRental_Team1_UI.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    [Route("[controller]s")]
    public class RentalController : Controller
    {
        private readonly IHttpClientFactory _http;

        public RentalController(IHttpClientFactory http)
        {
            _http = http;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals");

            if (response.IsSuccessStatusCode)
            {
                var rentals = response.Content.ReadFromJsonAsync<List<Rental>>().Result!;
                var model = new RentalListViewModel { Rentals = rentals };
                ViewData["Title"] = "Rentals";
                return View("All", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var client = _http.CreateClient();
            var role = User.FindFirstValue(ClaimTypes.Role);
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/{id}");

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                var model = new RentalViewModel { Rental = rental };
                ViewData["Title"] = "Rental Details";

                if (role == "Admin")
                {
                    return View("AdminRentalDetails", model);
                }
                else
                {
                    return View("UserRentalDetails", model);
                }
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("issue")]
        public async Task<IActionResult> Issue()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var customersResponse = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers");
            var equipmentResponse = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment/available");

            if (!customersResponse.IsSuccessStatusCode || !equipmentResponse.IsSuccessStatusCode)
            {
                var errorModel = new ErrorViewModel { RequestId = "Could not gather required information." };
                return View("Error", errorModel);
            }

            var customers = customersResponse.Content.ReadFromJsonAsync<List<Customer>>().Result!.Where(c => c.HasActiveRental == false).ToList();
            var equipment = equipmentResponse.Content.ReadFromJsonAsync<List<Equipment>>().Result!;

            ViewData["Title"] = "Issue Rental";
            var model = new RentalIssueViewModel { Rental = new Rental(), Customers = customers, Equipment = equipment };
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            if (role == "Admin")
            {
                return View("AdminIssue", model);
            }
            else
            {
                return View("UserIssue", model);
            }
        }

        [Authorize]
        [HttpPost("issue")]
        public async Task<IActionResult> Issue([FromBody] RentalIssueViewModel model)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (!ModelState.IsValid)
            {
                var customersResponse = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers");
                var equipmentResponse = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment/available");

                if (!customersResponse.IsSuccessStatusCode || !equipmentResponse.IsSuccessStatusCode)
                {
                    var errorModel = new ErrorViewModel { RequestId = "Could not gather required information." };
                    return View("Error", errorModel);
                }

                var customers = customersResponse.Content.ReadFromJsonAsync<List<Customer>>().Result!.Where(c => c.HasActiveRental == false).ToList();
                var equipment = equipmentResponse.Content.ReadFromJsonAsync<List<Equipment>>().Result!;

                model.Customers = customers;
                model.Equipment = equipment;
                var role = User.FindFirst(ClaimTypes.Role)!.Value;

                if (role == "Admin")
                {
                    return View("AdminIssue", model);
                }
                else
                {
                    return View("UserIssue", model);
                }
            }

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/rentals/issue", model.Rental);

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                return RedirectToAction("Details", new { id = rental.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = "Unable to issue Rental." };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("{id}/return")]
        public async Task<ActionResult> Return(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/{id}");

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                var conditions = Enum.GetValues<EquipmentCondition>().ToList();
                var model = new RentalReturnViewModel { Rental = rental, Conditions = conditions };

                ViewData["Title"] = "Return Rental";
                return View("Return", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = "Unable to return Rental." };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpPost("return")]
        public async Task<IActionResult> Return([FromBody] RentalReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var conditions = Enum.GetValues<EquipmentCondition>().ToList();
                model.Conditions = conditions;
                return View("Return", model);
            }

            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/rentals/return", model.Rental);

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                return RedirectToAction("Details", new { id = rental.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = "Unable to issue Rental." };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("completed")]
        public async Task<IActionResult> GetCompleted()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/completed");

            if (response.IsSuccessStatusCode)
            {
                var rentals = response.Content.ReadFromJsonAsync<List<Rental>>().Result!;
                var model = new RentalListViewModel { Rentals = rentals };
                ViewData["Title"] = "Completed Rentals";
                return View("Completed", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize("Admin")]
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/overdue");

            if (response.IsSuccessStatusCode)
            {
                var rentals = response.Content.ReadFromJsonAsync<List<Rental>>().Result!;
                var model = new RentalListViewModel { Rentals = rentals };
                ViewData["Title"] = "Overdue Rentals";
                return View("Overdue", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("equipment/{id}")]
        public async Task<IActionResult> GetEquipmentHistory(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/equipment/{id}");

            if (response.IsSuccessStatusCode)
            {
                var rentals = response.Content.ReadFromJsonAsync<List<Rental>>().Result!;
                var model = new RentalListViewModel { Rentals = rentals };
                ViewData["Title"] = "Equipment Rental History";
                return View("RentalHistory", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/rentals/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Title"] = "Extend Rental";
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                var model = new RentalViewModel { Rental = rental };
                return View("Extend", model);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/edit")]
        public async Task<IActionResult> Edit(int id, [FromBody] RentalViewModel model)
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

            var response = await client.PutAsJsonAsync($"{GlobalUsings.API_BASE_URL}/rentals/{id}", model.Rental);

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result!;
                return RedirectToAction("Details", new { id = rental.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ForceReturn(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.DeleteAsync($"{GlobalUsings.API_BASE_URL}/rentals/{id}");

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
