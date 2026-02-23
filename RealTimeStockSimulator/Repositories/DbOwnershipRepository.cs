using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbOwnershipRepository : DbBaseRepository, IOwnershipRepository
    {
        public DbOwnershipRepository(IConfiguration configuration) : base(configuration) { }

        public List<OwnershipAsset> GetAllOwnershipAssetsByUserId(int userId)
        {
            List<OwnershipAsset> ownershipAssets = new List<OwnershipAsset>();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Ownership.symbol, name, image_path, amount, type " +
                    "FROM Ownership " +
                    "JOIN Assets ON Ownership.symbol = Assets.symbol " +
                    "WHERE user_id = @UserId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ownershipAssets.Add(DataMapper.MapOwnershipAsset(reader));
                }
            }

            return ownershipAssets;
        }

        public OwnershipAsset? GetOwnershipAssetByUserId(int userId, string symbol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Ownership.symbol, amount, type " +
                     "FROM Ownership " +
                     "JOIN Assets ON Ownership.symbol = Assets.symbol " +
                     "WHERE user_id = @UserId AND Ownership.symbol = @Symbol;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", symbol);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return DataMapper.MapOwnershipAsset(reader);
                }
            }

            return null;
        }

        public void AddOwnershipAssetToUserId(int userId, OwnershipAsset asset)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Ownership(user_id, symbol, amount) " +
                    $"VALUES (@UserId, @Symbol, @Amount);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", asset.Symbol);
                command.Parameters.AddWithValue("@Amount", asset.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void UpdateOwnershipAsset(int userId, OwnershipAsset asset)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Ownership " +
                    "SET amount = @Amount " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", asset.Symbol);
                command.Parameters.AddWithValue("@Amount", asset.Amount);
                command.Connection.Open();

                command.ExecuteScalar();
            }
        }

        public void RemoveOwnershipAssetFromUserId(int userId, OwnershipAsset asset)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Ownership " +
                    "WHERE user_id = @UserId AND symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", asset.Symbol);
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
                    "JOIN Assets ON Assets.symbol = Ownership.symbol " +
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

                    if (ownerships.AssetsDictionary.ContainsKey(symbol) == false)
                    {
                        ownerships.AssetsDictionary[symbol] = DataMapper.MapAsset(reader);
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
