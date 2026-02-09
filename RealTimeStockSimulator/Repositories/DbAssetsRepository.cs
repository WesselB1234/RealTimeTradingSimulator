using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Helpers;

namespace RealTimeStockSimulator.Repositories
{
    public class DbAssetsRepository : DbBaseRepository, IAssetsRepository
    {
        public DbAssetsRepository(IConfiguration configuration) : base(configuration) { }

        public int AddTradable(Asset tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Assets(symbol, name, image_path, type) " +
                    $"VALUES (@Symbol, @Name, @ImagePath, @Type);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Name", (tradable.Name == null ? DBNull.Value : tradable.Name));
                command.Parameters.AddWithValue("@ImagePath", (tradable.ImagePath == null ? DBNull.Value : tradable.ImagePath));
                command.Parameters.AddWithValue("@Type", tradable.Type.ToString());

                command.Connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public List<Asset> GetAllTradables()
        {
            List<Asset> tradables = new List<Asset>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, name, image_path, type FROM Assets";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Asset tradable = DataMapper.MapTradable(reader);
                    tradables.Add(tradable);
                }
            }

            return tradables;
        }

        public Asset? GetTradableBySymbol(string symbol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, name, image_path, type FROM Assets WHERE symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Symbol", symbol);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return DataMapper.MapTradable(reader);
                }
            }

            return null;
        }
    }
}
