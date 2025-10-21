using Microsoft.AspNetCore.Mvc;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
    }
}
