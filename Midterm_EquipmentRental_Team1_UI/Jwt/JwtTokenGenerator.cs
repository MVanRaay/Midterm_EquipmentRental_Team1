using Microsoft.IdentityModel.Tokens;
using Midterm_EquipmentRental_Team1_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Midterm_EquipmentRental_Team1_UI.Jwt
{
    public class JwtTokenGenerator
    {
        public static string GenerateToken(AppUser user, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SuperSecretKeyForJwt123456789012"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7025",
                audience: "https://localhost:7088",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
