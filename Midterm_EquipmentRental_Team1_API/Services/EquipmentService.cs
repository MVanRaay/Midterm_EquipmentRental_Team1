using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;

namespace Midterm_EquipmentRental_Team1_API.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        return _unitOfWork.Equipment.GetAll();
    }

    public Equipment? GetEquipmentById(int id)
    {
        return _unitOfWork.Equipment.GetById(id);
    }

    public bool CreateEquipment(Equipment equipment)
    {
        equipment.CreatedAt = DateTime.Now;
        equipment.Condition = EquipmentCondition.New;
        equipment.IsAvailable = true;
        _unitOfWork.Equipment.Add(equipment);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool UpdateEquipment(int id, Equipment equipment)
    {
        equipment.Id = id;
        _unitOfWork.Equipment.Update(equipment);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool DeleteEquipment(int id)
    {
        Equipment equipment = _unitOfWork.Equipment.GetById(id)!;
        if (equipment == null) return false;
        _unitOfWork.Equipment.Delete(equipment);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public IEnumerable<Equipment> GetAvailableEquipment()
    {
        return _unitOfWork.Equipment.GetAll().Where(e => e.IsAvailable);
    }

    public IEnumerable<Equipment> GetRentedEquipment()
    {
        return _unitOfWork.Equipment.GetAll().Where(e => !e.IsAvailable);
    }
}
