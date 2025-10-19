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

    [HttpGet]
    public ActionResult GetAll()
    {
        IEnumerable<Rental> rentals = _service.GetAll();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        Rental rental = _service.GetById(id)!;
        return rental != null ? Ok(rental) : NotFound();
    }

    [HttpPost("issue")]
    public ActionResult Issue([FromBody] Rental rental)
    {
        bool success = _service.Issue(rental);
        return success ? Ok(rental) : NotFound();
    }

    [HttpPost("return")]
    public ActionResult Return([FromBody] Rental rental)
    {
        bool success = _service.Return(rental);
        return success ? Ok(rental) : NotFound();
    }

    [HttpGet("active")]
    public ActionResult GetActive()
    {
        IEnumerable<Rental> rentals = _service.GetActiveRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [HttpGet("completed")]
    public ActionResult GetCompleted()
    {
        IEnumerable<Rental> rentals = _service.GetCompletedRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [HttpGet("overdue")]
    public ActionResult GetOverdue()
    {
        IEnumerable<Rental> rentals = _service.GetOverdueRentals();
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [HttpGet("equipment/{equipmentId}")]
    public ActionResult GetRentalHistory(int equipmentId)
    {
        IEnumerable<Rental> rentals = _service.GetRentalsForEquipment(equipmentId);
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [HttpPut("{id}")]
    public ActionResult ExtendRental(int id, [FromBody] Rental rental)
    {
        rental.Id = id;
        bool success = _service.ExtendRental(rental);
        return success ? Ok(rental) : NotFound();
    }

    [HttpDelete("{id}")]
    public ActionResult ForceCancel(int id)
    {
        bool success = _service.ForceCancelRental(id);
        return success ? Ok(id) : NotFound();
    }
}
