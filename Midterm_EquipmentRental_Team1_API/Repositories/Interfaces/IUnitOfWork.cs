using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

public interface IUnitOfWork
{
    ICrudRepository<Equipment> Equipment { get; }
    ICrudRepository<Customer> Customers { get; }
    ICrudRepository<Rental> Rentals { get; }
    int Complete();
}
