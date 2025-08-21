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

                while (reader.Read())
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

                while (reader.Read())
                {
                    return _dataMapper.MapUser(reader);
                }
            }

            return null;
        }
    }
}
