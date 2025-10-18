using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAll();
    Customer? GetById(int id);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
}
