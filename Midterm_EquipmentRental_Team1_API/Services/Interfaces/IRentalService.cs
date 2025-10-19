using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces;

public interface IRentalService
{
    IEnumerable<Rental> GetAll();
    Rental? GetById(int id);
    bool Issue(Rental rental);
    bool Return(Rental rental);
    IEnumerable<Rental> GetActiveRentals();
    IEnumerable<Rental> GetCompletedRentals();
    IEnumerable<Rental> GetOverdueRentals();
    IEnumerable<Rental> GetRentalsForEquipment(int equipmentId);
    bool ExtendRental(Rental rental);
    bool ForceCancelRental(int id);
}
