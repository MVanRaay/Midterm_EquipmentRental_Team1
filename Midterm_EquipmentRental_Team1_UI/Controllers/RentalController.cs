using Microsoft.AspNetCore.Mvc;

namespace Midterm_EquipmentRental_Team1_UI.Controllers
{
    public class RentalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
