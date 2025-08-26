using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IUsersService
    {
        List<User> GetAllUsers();
        User? GetUserByName(string userName);
        User? GetUserByLoginCredentials(string userName, string password);
        User? GetUserByUserId(int userId);
        int AddUser(User user);
        void UpdateUser(User user);
    }
}
