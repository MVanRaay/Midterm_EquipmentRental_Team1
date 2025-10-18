using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

public interface IEquipmentRepository
{
    IEnumerable<Equipment> GetAll();
    Equipment? GetById(int id);
    void Add(Equipment equipment);
    void Update(Equipment equipment);
    void Delete(Equipment equipment);
}
