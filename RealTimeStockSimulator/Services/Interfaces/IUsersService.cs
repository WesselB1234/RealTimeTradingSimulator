using RealTimeStockSimulator.Models;
using System.Security.Claims;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IUsersService
    {
        List<UserAccount> GetAllUsers();
        UserAccount? GetUserByName(string userName);
        UserAccount? GetUserByLoginCredentials(string userName, string password);
        UserAccount? GetUserByUserId(int userId);
        int AddUser(UserAccount user);
        void UpdateUser(UserAccount user);
        ClaimsPrincipal GetClaimsPrincipleFromUser(UserAccount user);
    }
}
