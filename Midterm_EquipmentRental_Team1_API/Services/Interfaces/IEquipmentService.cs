using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces;

public interface IEquipmentService
{
    IEnumerable<Equipment> GetAllEquipment();
    Equipment? GetEquipmentById(int id);
    bool CreateEquipment(Equipment equipment);
    bool UpdateEquipment(int id, Equipment equipment);
    bool DeleteEquipment(int id);
    public IEnumerable<Equipment> GetAvailableEquipment();
    IEnumerable<Equipment> GetRentedEquipment();
}
