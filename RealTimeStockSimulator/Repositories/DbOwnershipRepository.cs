using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbOwnershipRepository : DbBaseRepository, IOwnershipsRepository
    {
        public DbOwnershipRepository(IConfiguration configuration, IDataMapper dataMapper) : base(configuration, dataMapper) { }

        public Ownership GetOwnershipByUser(UserAccount user)
        {
            Ownership ownership = new Ownership(user, new List<OwnershipTradable>());
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, amount " +
                    "FROM Ownership " +
                    "WHERE user_id = @UserId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", ownership.User.UserId);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ownership.Tradables.Add(_dataMapper.MapOwnershipTradable(reader));
                }
            }

            return ownership;
        }

        public OwnershipTradable? GetOwnershipTradableByUserId(int userId, string symbol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, amount " +
                    "FROM Ownership " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", symbol);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return _dataMapper.MapOwnershipTradable(reader);
                }
            }

            return null;
        }

        public void AddOwnershipTradableToUser(UserAccount user, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Ownership(user_id, symbol, amount) " +
                    $"VALUES (@UserId, @Symbol, @Amount);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Amount", tradable.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void UpdateOwnershipTradable(UserAccount user, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Ownership " +
                    "SET amount = @Amount " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Amount", tradable.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void RemoveOwnershipTradableFromUser(UserAccount user, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Ownership " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }
    }
}
