using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

public interface IRentalRepository
{
    IEnumerable<Rental> GetAll();
    Rental? GetById(int id);
    void Add(Rental rental);
    void Update(Rental rental);
    void Delete(Rental rental);
}
