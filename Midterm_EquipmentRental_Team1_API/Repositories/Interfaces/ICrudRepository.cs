using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces
{
    public interface ICrudRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Add(T customer);
        void Update(T customer);
        void Delete(T customer);
    }
}
