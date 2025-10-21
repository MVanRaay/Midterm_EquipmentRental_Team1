using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;
using Midterm_EquipmentRental_Team1_Models;
using System.Security.Claims;

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
        var userIdClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        IEnumerable<Rental> rentals;

        if (roleClaim == "Admin")
        {
            rentals = _service.GetAll();
        }
        else
        {
            rentals = _service.GetAll().Where(r => r.CustomerId == userId);
        }

        return rentals.Any() ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var userClaims = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaims = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userClaims)) return Unauthorized();

        int userId = int.Parse(userClaims);

        Rental rental = _service.GetById(id)!;

        if (rental == null) return NotFound();

        if (roleClaims != "Admin" && userId != rental.CustomerId) return Unauthorized();

        return Ok(rental);
    }

    [Authorize]
    [HttpPost("issue")]
    public ActionResult Issue([FromBody] Rental rental)
    {
        var userClaims = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaims = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userClaims)) return Unauthorized();

        int userId = int.Parse(userClaims);

        if (roleClaims != "Admin" && userId != rental.CustomerId) return Unauthorized();

        bool success = _service.Issue(rental);
        return success ? Ok(rental) : NotFound();
    }

    [Authorize]
    [HttpPost("return")]
    public ActionResult Return([FromBody] Rental rental)
    {
        var userClaims = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaims = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userClaims)) return Unauthorized();

        int userId = int.Parse(userClaims);

        if (roleClaims != "Admin" && userId != rental.CustomerId) return Unauthorized();

        bool success = _service.Return(rental);
        return success ? Ok(rental) : NotFound();
    }

    [Authorize]
    [HttpGet("active")]
    public ActionResult GetActive()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        IEnumerable<Rental> rentals;

        if (roleClaim == "Admin")
        {
            rentals = _service.GetActiveRentals();
        }
        else
        {
            rentals = _service.GetActiveRentals().Where(r => r.CustomerId == userId);
        }

        return rentals.Any() ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("completed")]
    public ActionResult GetCompleted()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        IEnumerable<Rental> rentals;

        if (roleClaim == "Admin")
        {
            rentals = _service.GetActiveRentals();
        }
        else
        {
            rentals = _service.GetActiveRentals().Where(r => r.CustomerId == userId);
        }

        return rentals.Any() ? Ok(rentals) : NotFound();
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
        var userIdClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        int userId = int.Parse(userIdClaim);
        IEnumerable<Rental> rentals;

        if (roleClaim == "Admin")
        {
            rentals = _service.GetRentalsForEquipment(equipmentId);
        }
        else
        {
            rentals = _service.GetRentalsForEquipment(equipmentId).Where(r => r.CustomerId == userId);
        }

        return rentals.Any() ? Ok(rentals) : NotFound();
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
