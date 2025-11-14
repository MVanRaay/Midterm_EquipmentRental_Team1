using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm_EquipmentRental_Team1_API.Data;
using Midterm_EquipmentRental_Team1_Models;


namespace Midterm_EquipmentRental_Team1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser([FromBody] AppUser incoming)
        {
            if (incoming.Email == null) return BadRequest("Email is required");

            var existing = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == incoming.Email);

            if (existing == null)
            {
                existing = new AppUser
                {
                    Email = incoming.Email,
                    ExternalProvider = incoming.ExternalProvider,
                    ExternalId = incoming.ExternalId,
                    Role = "User"
                };

                _context.AppUsers.Add(existing);
                await _context.SaveChangesAsync();
            }

            return Ok(existing);
        }
    }
}
