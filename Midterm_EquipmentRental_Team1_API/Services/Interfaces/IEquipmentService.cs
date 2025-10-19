using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces;

public interface IEquipmentService : ICrudService<Equipment>
{
    public IEnumerable<Equipment> GetAvailableEquipment();
    IEnumerable<Equipment> GetRentedEquipment();
}
