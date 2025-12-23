using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        List<UserAccount> GetAllUsers();
        UserAccount? GetUserByName(string userName);
        UserAccount? GetUserByLoginCredentials(string userName, string password);
        UserAccount? GetUserByUserId(int userId);
        int AddUser(UserAccount user);
        void UpdateBalanceByUserId(int userId, decimal newBalance);
    }
}
