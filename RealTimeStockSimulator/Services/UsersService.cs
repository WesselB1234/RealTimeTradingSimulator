using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;
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

        public int AddUser(User user)
        {
            if (GetUserByName(user.UserName) != null)
            {
                throw new Exception("User already exists.");
            }

            user.Password = HashPassword(user.Password);

            return _usersRepository.AddUser(user);
        }

        public User? GetUserByLoginCredentials(string userName, string password)
        {
            return _usersRepository.GetUserByLoginCredentials(userName, HashPassword(password));
        }

        public User? GetUserByName(string userName)
        {
            return _usersRepository.GetUserByName(userName);
        }

        public void UpdateUser(User user)
        {
            _usersRepository.UpdateUser(user);
        }

        public List<User> GetAllUsers()
        {
            return _usersRepository.GetAllUsers();
        }

        public User? GetUserByUserId(int userId)
        {
            return _usersRepository.GetUserByUserId(userId);
        }
    }
}
