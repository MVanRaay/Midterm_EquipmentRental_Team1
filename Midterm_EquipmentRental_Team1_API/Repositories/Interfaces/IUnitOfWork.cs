namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

public interface IUnitOfWork
{
    IEquipmentRepository Equipment { get; }
    ICustomerRepository Customers { get; }
    IRentalRepository Rentals { get; }
    int Complete();
}
