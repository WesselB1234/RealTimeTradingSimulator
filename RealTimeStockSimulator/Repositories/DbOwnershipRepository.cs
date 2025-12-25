using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbOwnershipRepository : DbBaseRepository, IOwnershipsRepository
    {
        public DbOwnershipRepository(IConfiguration configuration) : base(configuration) { }

        public List<OwnershipTradable> GetAllOwnershipTradablesByUserId(int userId)
        {
            List<OwnershipTradable> ownershipTradables = new List<OwnershipTradable>();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, amount " +
                    "FROM Ownership " +
                    "WHERE user_id = @UserId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ownershipTradables.Add(DataMapper.MapOwnershipTradable(reader));
                }
            }

            return ownershipTradables;
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
                    return DataMapper.MapOwnershipTradable(reader);
                }
            }

            return null;
        }

        public void AddOwnershipTradableToUserId(int userId, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Ownership(user_id, symbol, amount) " +
                    $"VALUES (@UserId, @Symbol, @Amount);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Amount", tradable.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void UpdateOwnershipTradable(int userId, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Ownership " +
                    "SET amount = @Amount " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Amount", tradable.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void RemoveOwnershipTradableFromUserId(int userId, OwnershipTradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Ownership " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public List<Ownership> GetOrderedOwnershipsPagnated(int userId, int pageSize, int currentPage)
        {
            throw new NotImplementedException();
        }
    }
}
