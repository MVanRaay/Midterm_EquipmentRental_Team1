using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomerController(ICustomerService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult GetAll()
    {
        IEnumerable<Customer> customers = _service.GetAll();
        return customers.ToList().Count > 0 ? Ok(customers) : NotFound();
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        Customer customer = _service.GetById(id)!;
        return customer != null ? Ok(customer) : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Add([FromBody] Customer customer)
    {
        bool success = _service.Create(customer);
        return success ? CreatedAtAction("Get", customer.Id) : BadRequest();
    }

    [Authorize]
    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Customer customer)
    {
        bool success = _service.Update(id, customer);
        return success ? Get(id) : BadRequest();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        bool success = _service.Delete(id);
        return success ? Ok() : NotFound();
    }

    [Authorize]
    [HttpGet("{id}/rentals")]
    public ActionResult GetCustomerRentalHistory(int id)
    {
        IEnumerable<Rental> rentals = _service.GetRentalsByCustomerId(id);
        return rentals.ToList().Count > 0 ? Ok(rentals) : NotFound();
    }

    [Authorize]
    [HttpGet("{id}/active-rental")]
    public ActionResult GetCustomerActiveRental(int id)
    {
        Rental rental = _service.GetActiveRentalByCustomerId(id)!;
        return rental != null ? Ok(rental) : NotFound();
    }
}
