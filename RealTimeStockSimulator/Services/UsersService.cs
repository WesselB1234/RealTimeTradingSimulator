using Microsoft.AspNetCore.Identity;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealTimeStockSimulator.Services
{
    public class UsersService : IUsersService
    {
        IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public int AddUser(UserAccount user)
        {
            if (GetUserByName(user.UserName) != null)
            {
                throw new Exception("User already exists.");
            }

            user.Password = HashPassword(user.Password);

            return _usersRepository.AddUser(user);
        }

        public UserAccount? GetUserByLoginCredentials(string userName, string password)
        {
            return _usersRepository.GetUserByLoginCredentials(userName, HashPassword(password));
        }

        public UserAccount? GetUserByName(string userName)
        {
            return _usersRepository.GetUserByName(userName);
        }

        public void UpdateBalanceByUserId(int userId, decimal newBalance)
        {
            _usersRepository.UpdateBalanceByUserId(userId, newBalance);
        }

        public List<UserAccount> GetAllUsers()
        {
            return _usersRepository.GetAllUsers();
        }

        public UserAccount? GetUserByUserId(int userId)
        {
            return _usersRepository.GetUserByUserId(userId);
        }

        public ClaimsPrincipal GetClaimsPrincipleFromUser(UserAccount user)
        {
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Money", user.Money.ToString()),
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

            return new ClaimsPrincipal(identity);
        }

        public UserAccount GetUserFromClaimsPrinciple(ClaimsPrincipal claims)
        {
            return new UserAccount
            {
                UserId = int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value),
                UserName = claims.FindFirst(ClaimTypes.Name).Value.ToString(),
                Email = claims.FindFirst(ClaimTypes.Email).Value.ToString(),
                Money = decimal.Parse(claims.FindFirst("Money").Value)
            };
        }
    }
}
