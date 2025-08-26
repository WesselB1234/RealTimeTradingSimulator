using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbUsersRepository : DbBaseRepository, IUsersRepository
    {
        public DbUsersRepository(IConfiguration configuration, IDataMapper dataMapper) : base(configuration, dataMapper) {}

        public int AddUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users(username, email, password, money) " +
                    $"VALUES (@Username, @Email, @Password, @Money);" +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Money", user.Money);

                command.Connection.Open();
                int? userId = Convert.ToInt32(command.ExecuteScalar());

                if (userId == null)
                {
                    throw new Exception("Insert user did not return a valid user_id.");
                }

                return (int)userId;
            }
        }

        public User? GetUserByName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT user_id, username, email, password, money " +
                    "FROM Users " +
                    "WHERE username = @UserName";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", userName);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return _dataMapper.MapUser(reader);
                }
            }

            return null;
        }

        public User? GetUserByLoginCredentials(string userName, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT user_id, username, email, password, money " +
                    "FROM Users " +
                    "WHERE username = @UserName AND password = @Password";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return _dataMapper.MapUser(reader);
                }
            }

            return null;
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users " +
                    "SET username = @UserName, email = @Email, password = @Password, money = @Money " +
                    "WHERE user_id = @UserId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Money", user.Money);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT user_id, username, email, password, money FROM Users;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(_dataMapper.MapUser(reader));
                }
            }

            return users;
        }

        public User? GetUserByUserId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT user_id, username, email, password, money " +
                    "FROM Users " +
                    "WHERE user_id = @UserId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return _dataMapper.MapUser(reader);
                }
            }

            return null;
        }
    }
}
