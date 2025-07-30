using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace RealTimeStockSimulator.Repositories
{
    public class DbTradablesRepository : DbBaseRepository, ITradablesRepository
    {
        public DbTradablesRepository(IConfiguration configuration) : base(configuration) { }

        private Tradable ReadTradable(SqlDataReader reader)
        {
            string symbol = (string)reader["symbol"];

            return new Tradable(symbol);
        }

        public List<Tradable> GetAllTradables()
        {
            List<Tradable> tradables = new List<Tradable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Tradables";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tradable tradable = ReadTradable(reader);
                        tradables.Add(tradable);
                    }
                }
            }

            return tradables;
        }
    }
}
