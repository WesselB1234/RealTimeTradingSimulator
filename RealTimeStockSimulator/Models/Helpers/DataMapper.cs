using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Enums;

namespace RealTimeStockSimulator.Models.Helpers
{
    public static class DataMapper
    {
        public static UserAccount MapUserWithPassword(SqlDataReader reader)
        {
            int userId = (int)reader["user_id"];
            string userName = (string)reader["username"];
            string email = (string)reader["email"];
            string password = (string)reader["password"];
            decimal money = (decimal)reader["money"];

            return new UserAccount(userId, userName, email, password, money);
        }

        public static UserAccount MapUser(SqlDataReader reader)
        {
            int userId = (int)reader["user_id"];
            string userName = (string)reader["username"];
            string email = (string)reader["email"];
            decimal money = (decimal)reader["money"];

            return new UserAccount(userId, userName, email, money);
        }

        private static bool GetIsNullReaderColumn(SqlDataReader reader, string columnName)
        {
            try
            {
                return reader.IsDBNull(reader.GetOrdinal(columnName));
            }
            catch (IndexOutOfRangeException)
            {
                return true;
            }
        }

        public static Asset MapAsset(SqlDataReader reader)
        {
            string symbol = (string)reader["symbol"];

            string? name = GetIsNullReaderColumn(reader, "name")
                ? null
                : (string)reader["name"];

            string? imagePath = GetIsNullReaderColumn(reader, "image_path")
                ? null
                : (string)reader["image_path"];

            AssetType type = (AssetType)Enum.Parse(typeof(AssetType), (string)reader["type"]);

            return new Asset(symbol, name, imagePath, type);
        }

        public static OwnershipAsset MapOwnershipAsset(SqlDataReader reader)
        {
            int amount = (int)reader["amount"];

            return MapOwnershipAssetByAsset(MapAsset(reader), amount);
        }

        public static MarketTransactionAsset MapMarketTransactionAsset(SqlDataReader reader)
        {
            int transactionId = (int)reader["transaction_id"];
            Asset asset = MapAsset(reader);
            decimal price = (decimal)reader["price"];
            MarketTransactionStatus status = (MarketTransactionStatus)Enum.Parse(typeof(MarketTransactionStatus), (string)reader["status"]);
            int amount = (int)reader["amount"];
            DateTime date = (DateTime)reader["date"];

            return new MarketTransactionAsset(transactionId, asset, price, status, amount, date);
        }

        public static OwnershipAsset MapOwnershipAssetByAsset(Asset asset, int amount)
        {
            return new OwnershipAsset(asset.Symbol, asset.Name, asset.ImagePath, asset.Type, amount);
        }
    }
}
