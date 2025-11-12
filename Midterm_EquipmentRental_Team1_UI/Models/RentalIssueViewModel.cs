using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_UI.Models
{
    public class RentalIssueViewModel
    {
        public Rental? Rental { get; set; }
        public List<Customer> Customers { get; set; } = [];
        public List<Equipment> Equipment { get; set; } = [];
    }
}
