namespace Midterm_EquipmentRental_Team1_Models;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
    public bool HasActiveRental { get; set; }
}
