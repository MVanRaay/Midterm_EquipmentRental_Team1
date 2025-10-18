namespace Midterm_EquipmentRental_Team1_Models;

public class Rental
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public int CustomerId { get; set; }
    public bool Status { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public bool IsOverdue { get => DueDate < DateTime.Now && ReturnedAt == null; }
    public Equipment? Equipment { get; set; }
    public Customer? Customer { get; set; }
}
