using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midterm_EquipmentRental_Team1_Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = "User"; // "Admin" or "User"
        public string? ExternalProvider { get; set; } = "Google";
        public string? ExternalId { get; set; }
    }
}
