using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetAllCustomers();
    Customer? GetCustomerById(int id);
    bool CreateCustomer(Customer customer);
    bool UpdateCustomer(int id, Customer customer);
    bool DeleteCustomer(int id);
    IEnumerable<Rental> GetRentalsByCustomerId(int id);
    Rental? GetActiveRentalByCustomerId(int id);
}
