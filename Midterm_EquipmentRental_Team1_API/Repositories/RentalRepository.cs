using Midterm_EquipmentRental_Team1_API.Data;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Midterm_EquipmentRental_Team1_API.Repositories;

public class RentalRepository : ICrudRepository<Rental>
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Rental> GetAll()
    {
        return _context.Rentals.Include(r => r.Customer).Include(r => r.Equipment);
    }
    public Rental? GetById(int id)
    {
        return _context.Rentals.Include(r => r.Customer).Include(r => r.Equipment).FirstOrDefault(r => r.Id == id);
    }
    public void Add(Rental rental)
    {
        _context.Rentals.Add(rental);
    }
    public void Update(Rental rental)
    {
        _context.Rentals.Update(rental);
    }
    public void Delete(Rental rental)
    {
        _context.Rentals.Remove(rental);
    }
}
