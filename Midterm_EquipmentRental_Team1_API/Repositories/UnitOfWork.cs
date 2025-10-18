using Midterm_EquipmentRental_Team1_API.Data;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team1_API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IEquipmentRepository Equipment { get; set; }
    public ICustomerRepository Customers { get; set; }
    public IRentalRepository Rentals { get; set; }

    public UnitOfWork(IEquipmentRepository equipmentRepository, ICustomerRepository customerRepository, IRentalRepository rentalRepository, AppDbContext context)
    {
        Equipment = equipmentRepository;
        Customers = customerRepository;
        Rentals = rentalRepository;
        _context = context;
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }
}
