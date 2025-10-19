using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces;

public interface ICustomerService : ICrudService<Customer>
{
    IEnumerable<Rental> GetRentalsByCustomerId(int id);
    Rental? GetActiveRentalByCustomerId(int id);
}
