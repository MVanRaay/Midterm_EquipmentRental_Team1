using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;
using Midterm_EquipmentRental_Team1_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Midterm_EquipmentRental_Team1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerService _customerRepository;

        public AuthController(ICustomerService customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties 
            { 
                RedirectUri = returnUrl 
            }, "oidc");
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
            return Ok();
        }

        [HttpGet("user-info")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            // Extract email and sub claims
            var email = User.FindFirst("email")?.Value;
            var sub = User.FindFirst("sub")?.Value;
            var name = User.FindFirst("name")?.Value;

            return Ok(new
            {
                Email = email,
                Sub = sub,
                Name = name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        //[HttpPost("login")]
        //public ActionResult<string> Login([FromBody] LoginRequest request)
        //{
        //    var customer = _customerRepository.GetAll().FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        //    if (customer == null)
        //    {
        //        return Unauthorized("Invalid username or password");
        //    }

            //    var token = GenerateJwtToken(customer);

            //    return Ok(new { Token = token });
            //}

            //private object GenerateJwtToken(Customer customer)
            //{
            //    var claims = new[]
            //    {
            //        new Claim(ClaimTypes.Name, customer.Id.ToString()),
            //        new Claim(ClaimTypes.Role, customer.Role)
            //    };

            //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("MidtermTeam1Section2SuperSecretKey123456"));
            //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //    var token = new JwtSecurityToken(
            //        claims: claims,
            //        expires: DateTime.Now.AddMinutes(30),
            //        signingCredentials: creds);

            //    return new JwtSecurityTokenHandler().WriteToken(token);
            //}
    }
}
