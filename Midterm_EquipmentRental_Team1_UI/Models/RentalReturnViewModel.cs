using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_UI.Models
{
    public class RentalReturnViewModel
    {
        public Rental? Rental { get; set; }
        public List<EquipmentCondition> Conditions { get; set; } = [];
    }
}
