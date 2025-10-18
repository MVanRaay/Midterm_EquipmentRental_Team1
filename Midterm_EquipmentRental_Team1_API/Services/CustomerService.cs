using Midterm_EquipmentRental_Team1_Models;
using Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;
using Midterm_EquipmentRental_Team1_API.Services.Interfaces;

namespace Midterm_EquipmentRental_Team1_API.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _unitOfWork.Customers.GetAll();
    }

    public Customer? GetCustomerById(int id)
    {
        return _unitOfWork.Customers.GetById(id);
    }

    public bool CreateCustomer(Customer customer)
    {
        _unitOfWork.Customers.Add(customer);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool UpdateCustomer(int id, Customer customer)
    {
        customer.Id = id;
        _unitOfWork.Customers.Update(customer);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public bool DeleteCustomer(int id)
    {
        Customer customer = _unitOfWork.Customers.GetById(id)!;
        if (customer == null) return false;
        _unitOfWork.Customers.Delete(customer);
        int result = _unitOfWork.Complete();
        return result > 0;
    }

    public IEnumerable<Rental> GetRentalsByCustomerId(int id)
    {
        return _unitOfWork.Rentals.GetAll().Where(r => r.CustomerId == id);
    }

    public Rental? GetActiveRentalByCustomerId(int id)
    {
        return _unitOfWork.Rentals.GetAll().FirstOrDefault(r => r.CustomerId == id && r.Status == true);
    }
}
