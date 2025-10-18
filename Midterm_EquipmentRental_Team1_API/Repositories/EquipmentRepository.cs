using Midterm_EquipmentRental_Team1_API.Data;
using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

namespace Midterm_EquipmentRental_Team1_API.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext _context;

    public EquipmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Equipment> GetAll()
    {
        return _context.Equipment;
    }

    public Equipment? GetById(int id)
    {
        return _context.Equipment.FirstOrDefault(e => e.Id == id);
    }

    public void Add(Equipment equipment)
    {
        _context.Equipment.Add(equipment);
    }

    public void Update(Equipment equipment)
    {
        _context.Equipment.Update(equipment);
    }

    public void Delete(Equipment equipment)
    {
        _context.Equipment.Remove(equipment);
    }
}
