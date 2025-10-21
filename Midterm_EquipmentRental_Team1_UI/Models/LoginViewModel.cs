namespace Midterm_EquipmentRental_Team1_UI.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
