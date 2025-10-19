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

    public IEnumerable<Equipment> GetAll()
    {
        return _unitOfWork.Equipment.GetAll();
    }

    public Equipment? GetById(int id)
    {
        return _unitOfWork.Equipment.GetById(id);
    }

    public bool Create(Equipment equipment)
    {
        equipment.CreatedAt = DateTime.Now;
        equipment.Condition = EquipmentCondition.New;
        equipment.IsAvailable = true;
        _unitOfWork.Equipment.Add(equipment);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool Update(int id, Equipment equipment)
    {
        equipment.Id = id;
        _unitOfWork.Equipment.Update(equipment);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool Delete(int id)
    {
        Equipment equipment = _unitOfWork.Equipment.GetById(id)!;
        if (equipment == null || _unitOfWork.Rentals.GetAll().Where(r => r.EquipmentId == equipment.Id && r.Status).Any()) return false;
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
