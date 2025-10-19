using Microsoft.AspNetCore.Mvc;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;

namespace Midterm_EquipmentRental_Team1_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _service;

    public EquipmentController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        IEnumerable<Equipment> equipment = _service.GetAll();
        return equipment.ToList().Count > 0 ? Ok(equipment) : NotFound();
    }

    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        Equipment equipment = _service.GetById(id)!;
        return equipment != null ? Ok(equipment) : NotFound();
    }

    [HttpPost]
    public ActionResult Add([FromBody] Equipment equipment)
    {
        bool success = _service.Create(equipment);
        return success ? CreatedAtAction("Get", equipment.Id) : BadRequest();
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Equipment equipment)
    {
        bool success = _service.Update(id, equipment);
        return success ? Get(id) : BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        bool success = _service.Delete(id);
        return success ? Ok() : NotFound();
    }

    [HttpGet("available")]
    public ActionResult GetAvailable()
    {
        IEnumerable<Equipment> equipment = _service.GetAvailableEquipment();
        return equipment.ToList().Count > 0 ? Ok(equipment) : NotFound();
    }

    [HttpGet("rented")]
    public ActionResult GetRented()
    {
        IEnumerable<Equipment> equipment = _service.GetRentedEquipment();
        return equipment.ToList().Count > 0 ? Ok(equipment) : NotFound();
    }
}
