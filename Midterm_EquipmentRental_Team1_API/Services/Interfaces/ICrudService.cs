namespace Midterm_EquipmentRental_Team1_API.Services.Interfaces
{
    public interface ICrudService<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        bool Create(T model);
        bool Update(int id, T model);
        bool Delete(int id);
    }
}
