using Midterm_EquipmentRental_Team1_Models;

namespace Midterm_EquipmentRental_Team1_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
    }
}
