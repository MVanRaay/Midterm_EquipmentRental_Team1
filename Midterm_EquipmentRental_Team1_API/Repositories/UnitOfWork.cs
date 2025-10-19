using Midterm_EquipmentRental_Team1_API.Data;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ICrudRepository<Equipment> Equipment { get; set; }
    public ICrudRepository<Customer> Customers { get; set; }
    public ICrudRepository<Rental> Rentals { get; set; }

    public UnitOfWork(ICrudRepository<Equipment> equipmentRepository, ICrudRepository<Customer> customerRepository, ICrudRepository<Rental> rentalRepository, AppDbContext context)
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
