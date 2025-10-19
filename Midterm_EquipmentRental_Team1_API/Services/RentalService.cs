using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;
using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services;

public class RentalService : IRentalService
{
    private readonly IUnitOfWork _unitOfWork;

    public RentalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Rental> GetAll()
    {
        return _unitOfWork.Rentals.GetAll();
    }

    public Rental? GetById(int id)
    {
        return _unitOfWork.Rentals.GetById(id);
    }

    public bool Issue(Rental rental)
    {
        Equipment equipment = _unitOfWork.Equipment.GetById(rental.EquipmentId)!;
        Customer customer = _unitOfWork.Customers.GetById(rental.CustomerId)!;
        if (equipment == null || customer == null) return false;
        if (!equipment.IsAvailable || customer.HasActiveRental) return false;

        equipment.IsAvailable = false;
        customer.HasActiveRental = true;
        rental.Status = true;
        rental.IssuedAt = DateTime.Now;
        rental.DueDate = DateTime.Now.Date.AddDays(3).AddHours(23).AddMinutes(59).AddSeconds(59);
        rental.ReturnedAt = null;
        _unitOfWork.Rentals.Add(rental);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool Return(Rental rental)
    {
        Equipment equipment = _unitOfWork.Equipment.GetById(rental.EquipmentId)!;
        Customer customer = _unitOfWork.Customers.GetById(rental.CustomerId)!;
        if (equipment == null || customer == null) return false;

        equipment.IsAvailable = true;
        customer.HasActiveRental = false;
        rental.Status = false;
        rental.ReturnedAt = DateTime.Now;
        _unitOfWork.Rentals.Update(rental);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public IEnumerable<Rental> GetActiveRentals()
    {
        return _unitOfWork.Rentals.GetAll().Where(r => r.Status);
    }

    public IEnumerable<Rental> GetCompletedRentals()
    {
        return _unitOfWork.Rentals.GetAll().Where(r => !r.Status);
    }

    public IEnumerable<Rental> GetOverdueRentals()
    {
        return _unitOfWork.Rentals.GetAll().Where(r => r.IsOverdue);
    }

    public IEnumerable<Rental> GetRentalsForEquipment(int equipmentId)
    {
        return _unitOfWork.Rentals.GetAll().Where(r => r.EquipmentId == equipmentId);
    }

    public bool ExtendRental(Rental rental)
    {
        rental.DueDate = rental.DueDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        _unitOfWork.Rentals.Update(rental);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool ForceCancelRental(int id)
    {
        Rental rental = _unitOfWork.Rentals.GetById(id)!;
        if (rental == null) return false;
        Equipment equipment = _unitOfWork.Equipment.GetById(rental.EquipmentId)!;;        
        Customer customer = _unitOfWork.Customers.GetById(rental.CustomerId)!;
        if (equipment == null || customer == null) return false;

        equipment.IsAvailable = true;
        customer.HasActiveRental = false;
        rental.Status = false;
        rental.ReturnedAt = DateTime.Now;
        _unitOfWork.Rentals.Update(rental);
        int result = _unitOfWork.Complete();
        return result > 0;
    }
}
