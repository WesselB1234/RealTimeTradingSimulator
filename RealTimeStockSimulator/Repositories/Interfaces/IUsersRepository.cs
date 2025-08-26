using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        List<User> GetAllUsers();
        User? GetUserByName(string userName);
        User? GetUserByLoginCredentials(string userName, string password);
        User? GetUserByUserId(int userId);
        int AddUser(User user);
        void UpdateUser(User user);
    }
}
