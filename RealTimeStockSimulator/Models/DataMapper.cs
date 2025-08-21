using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Interfaces;
using System.Data;

namespace RealTimeStockSimulator.Models
{
    public class DataMapper : IDataMapper
    {
        public User MapUser(SqlDataReader reader)
        {
            int userId = (int)reader["user_id"];
            string userName = (string)reader["username"];
            string email = (string)reader["email"];
            string password = (string)reader["password"];
            decimal money = (decimal)reader["money"];

            return new User(userId, userName, email, password, money);
        }

        public Tradable MapTradable(SqlDataReader reader)
        {
            string symbol = (string)reader["symbol"];

            return new Tradable(symbol);
        }
    }
}
