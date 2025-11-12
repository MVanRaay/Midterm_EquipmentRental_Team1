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
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _http;

        public CustomerController(IHttpClientFactory http)
        {
            _http = http;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> UserDashboard()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var id = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}/active-rental");

            if (response.IsSuccessStatusCode)
            {
                var rental = response.Content.ReadFromJsonAsync<Rental>().Result;
                var model = new RentalViewModel { Rental = rental };

                ViewData["Title"] = "User Dashboard";
                return View("UserDashboard", model);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var model = new RentalViewModel { Rental = null };

                ViewData["Title"] = "User Dashboard";
                return View("UserDashboard", model);
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/equipment/rented");

            if (response.IsSuccessStatusCode)
            {
                var equipment = response.Content.ReadFromJsonAsync<List<Equipment>>().Result!;
                var model = new EquipmentListViewModel { Equipment = equipment };

                ViewData["Title"] = "Admin Dashboard";
                return View("AdminDashboard", model);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var model = new EquipmentListViewModel { Equipment = [] };

                ViewData["Title"] = "Admin Dashboard";
                return View("AdminDashboard", model);
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }

        [Authorize]
        [HttpGet("{id}/rentals")]
        public async Task<IActionResult> MyRentals(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var userId = Int32.Parse(User.FindFirst(ClaimTypes.Name)!.Value);
            if (userId != id) return View("Error");

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}/rentals");

            if (response.IsSuccessStatusCode)
            {
                var rentals = response.Content.ReadFromJsonAsync<List<Rental>>().Result!;
                var model = new RentalListViewModel { Rentals = rentals };

                ViewData["Title"] = "My Rentals";
                return View("MyRentals", model);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var model = new RentalListViewModel { Rentals = [] };

                ViewData["Title"] = "My Rentals";
                return View("MyRentals", model);
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers");

            if (response.IsSuccessStatusCode)
            {
                var customers = response.Content.ReadFromJsonAsync<List<Customer>>().Result!;
                var model = new CustomerListViewModel { Customers = customers };
                ViewData["Title"] = "All Customers";
                return View("All", model);
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}");

            if (response.IsSuccessStatusCode)
            {
                var customer = response.Content.ReadFromJsonAsync<Customer>().Result!;
                var model = new CustomerViewModel { Customer = customer };
                ViewData["Title"] = "Customer Details";
                return View("Details", model);
            }
            else
            {
                var model = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("add")]
        public IActionResult Add()
        {
            ViewData["Title"] = "Add Equipment";
            var model = new CustomerViewModel();
            return View("Add", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CustomerViewModel model)
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

            var response = await client.PostAsJsonAsync($"{GlobalUsings.API_BASE_URL}/customers/add", model.Customer);

            if (response.IsSuccessStatusCode)
            {
                var customer = response.Content.ReadFromJsonAsync<Customer>().Result!;
                return RedirectToAction("Details", new { id = customer.Id });
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = "Unable to create new Customer" };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewData["Title"] = "Edit Customer";
                var equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                var model = new EquipmentViewModel { Equipment = equipment };
                var role = User.FindFirst(ClaimTypes.Role)!.Value;

                if (role == "Admin")
                {
                    return View("AdminEdit", model);
                }
                else
                {
                    return View("UserEdit", model);
                }
                
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize]
        [HttpPut("{id}/edit")]
        public async Task<IActionResult> Edit(int id, [FromBody] CustomerViewModel model)
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

            var response = await client.PutAsJsonAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}", model.Customer);

            if (response.IsSuccessStatusCode)
            {
                var equipment = response.Content.ReadFromJsonAsync<Equipment>().Result!;
                var viewModel = new EquipmentViewModel { Equipment = equipment };
                return RedirectToAction("Details", viewModel);
            }
            else
            {
                var errorModel = new ErrorViewModel { RequestId = response.StatusCode.ToString() };
                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _http.CreateClient();
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.DeleteAsync($"{GlobalUsings.API_BASE_URL}/customers/{id}");

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Successfully Deleted Customer";
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
