using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class RentalController : ControllerBase
{
    private readonly IRentalService _service;

    public RentalController(IRentalService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public ActionResult GetAll()
    {
        IEnumerable<Rental> rentals = _service.GetAll();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        Rental rental = _service.GetById(id)!;
        return rental != null ? Ok(rental) : NotFound();
    }

    [Authorize]
    [HttpPost("issue")]
    public ActionResult Issue([FromBody] Rental rental)
    {
        bool success = _service.Issue(rental);
        return success ? Ok(rental) : NotFound();
    }

    [Authorize]
    [HttpPost("return")]
    public ActionResult Return([FromBody] Rental rental)
    {
        bool success = _service.Return(rental);
        return success ? Ok(rental) : NotFound();
    }

    [Authorize]
    [HttpGet("active")]
    public ActionResult GetActive()
    {
        IEnumerable<Rental> rentals = _service.GetActiveRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("completed")]
    public ActionResult GetCompleted()
    {
        IEnumerable<Rental> rentals = _service.GetCompletedRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("overdue")]
    public ActionResult GetOverdue()
    {
        IEnumerable<Rental> rentals = _service.GetOverdueRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("equipment/{equipmentId}")]
    public ActionResult GetRentalHistory(int equipmentId)
    {
        IEnumerable<Rental> rentals = _service.GetRentalsForEquipment(equipmentId);
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public ActionResult ExtendRental(int id, [FromBody] Rental rental)
    {
        rental.Id = id;
        bool success = _service.ExtendRental(rental);
        return success ? Ok(rental) : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult ForceCancel(int id)
    {
        bool success = _service.ForceCancelRental(id);
        return success ? Ok(id) : NotFound();
    }
}
