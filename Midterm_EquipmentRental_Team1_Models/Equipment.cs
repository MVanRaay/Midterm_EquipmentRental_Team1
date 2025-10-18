namespace Midterm_EquipmentRental_Team1_Models;

public class Equipment
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal RentalPrice { get; set; }
    public bool IsAvailable { get; set; }
    public EquipmentCategory Category { get; set; }
    public EquipmentCondition Condition { get; set; }
    public DateTime? CreatedAt { get; set; }
}
