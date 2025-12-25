using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Enums;

namespace RealTimeStockSimulator.Models.Helpers
{
    public static class DataMapper
    {
        public static UserAccount MapUser(SqlDataReader reader)
        {
            int userId = (int)reader["user_id"];
            string userName = (string)reader["username"];
            string email = (string)reader["email"];
            string password = (string)reader["password"];
            decimal money = (decimal)reader["money"];

            return new UserAccount(userId, userName, email, password, money);
        }

        public static Tradable MapTradable(SqlDataReader reader)
        {
            string symbol = (string)reader["symbol"];

            return new Tradable(symbol);
        }

        public static OwnershipTradable MapOwnershipTradable(SqlDataReader reader)
        {
            string symbol = (string)reader["symbol"];
            int amount = (int)reader["amount"];

            return new OwnershipTradable(symbol, amount);
        }

        public static MarketTransactionTradable MapMarketTransactionTradable(SqlDataReader reader)
        {
            int transactionId = (int)reader["transaction_id"];
            Tradable tradable = MapTradable(reader);
            decimal price = (decimal)reader["price"];
            MarketTransactionStatus status = (MarketTransactionStatus)Enum.Parse(typeof(MarketTransactionStatus), (string)reader["status"]);
            int amount = (int)reader["amount"];
            DateTime date = (DateTime)reader["date"];

            return new MarketTransactionTradable(transactionId, tradable, price, status, amount, date);
        }

        public static OwnershipTradable MapOwnershipTradableByTradable(Tradable tradable, int amount)
        {
            return new OwnershipTradable(tradable.Symbol, amount);
        }
    }
}
