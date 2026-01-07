using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbOwnershipRepository : DbBaseRepository, IOwnershipRepository
    {
        public DbOwnershipRepository(IConfiguration configuration) : base(configuration) { }

        public List<OwnershipTradable> GetAllOwnershipTradablesByUserId(int userId)
        {
            List<OwnershipTradable> ownershipTradables = new List<OwnershipTradable>();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Ownership.symbol, name, image_path, amount, type " +
                    "FROM Ownership " +
                    "JOIN Tradables ON Ownership.symbol = Tradables.symbol " +
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
                string query = "SELECT Ownership.symbol, amount, type " +
                     "FROM Ownership " +
                     "JOIN Tradables ON Ownership.symbol = Tradables.symbol " +
                     "WHERE user_id = @UserId AND Ownership.symbol = @Symbol;";

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

        public MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage)
        {
            MultiOwnership ownerships = new MultiOwnership();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Ownership.user_id, Ownership.symbol, username, email, [money], amount, type " +
                    "FROM Ownership " +
                    "JOIN Tradables ON Tradables.symbol = Ownership.symbol " +
                    "JOIN Users ON Ownership.user_id = Users.user_id " +
                    "ORDER BY Ownership.user_id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Ownership? currentOwnership = null;

                while (reader.Read())
                {
                    UserAccount user = DataMapper.MapUser(reader);

                    if (currentOwnership == null)
                    {
                        currentOwnership = new Ownership();
                    }

                    if (currentOwnership.User == null)
                    {
                        currentOwnership.User = DataMapper.MapUser(reader);
                    }

                    if (currentOwnership.User.UserId != user.UserId)
                    {
                        ownerships.Ownerships.Add(currentOwnership);

                        currentOwnership = new Ownership();
                        currentOwnership.User = user;
                    }

                    string symbol = (string)reader["symbol"];
                    int amount = (int)reader["amount"];

                    if (ownerships.TradablesDictionary.ContainsKey(symbol) == false)
                    {
                        ownerships.TradablesDictionary[symbol] = DataMapper.MapTradable(reader);
                    }

                    currentOwnership.OwnedAmountOfSymbolDictionary[symbol] = amount;
                }

                if (currentOwnership != null) {
                    ownerships.Ownerships.Add(currentOwnership);
                }
            }

            return ownerships;
        }
    }
}
