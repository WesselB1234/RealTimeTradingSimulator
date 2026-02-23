using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Helpers;

namespace RealTimeStockSimulator.Repositories
{
    public class DbAssetsRepository : DbBaseRepository, IAssetsRepository
    {
        public DbAssetsRepository(IConfiguration configuration) : base(configuration) { }

        public int AddAsset(Asset asset)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Assets(symbol, name, image_path, type) " +
                    $"VALUES (@Symbol, @Name, @ImagePath, @Type);";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Symbol", asset.Symbol);
                command.Parameters.AddWithValue("@Name", (asset.Name == null ? DBNull.Value : asset.Name));
                command.Parameters.AddWithValue("@ImagePath", (asset.ImagePath == null ? DBNull.Value : asset.ImagePath));
                command.Parameters.AddWithValue("@Type", asset.Type.ToString());

                command.Connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public List<Asset> GetAllAssets()
        {
            List<Asset> assets = new List<Asset>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, name, image_path, type FROM Assets";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Asset asset = DataMapper.MapAsset(reader);
                    assets.Add(asset);
                }
            }

            return assets;
        }

        public Asset? GetAssetBySymbol(string symbol)
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
                    return DataMapper.MapAsset(reader);
                }
            }

            return null;
        }
    }
}
