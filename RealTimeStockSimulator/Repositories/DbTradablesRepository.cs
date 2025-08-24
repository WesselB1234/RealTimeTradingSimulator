using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbTradablesRepository : DbBaseRepository, ITradablesRepository
    {
        public DbTradablesRepository(IConfiguration configuration, IDataMapper dataMapper) : base(configuration, dataMapper) { }

        public List<Tradable> GetAllTradables()
        {
            List<Tradable> tradables = new List<Tradable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol FROM Tradables";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tradable tradable = _dataMapper.MapTradable(reader);
                        tradables.Add(tradable);
                    }
                }
            }

            return tradables;
        }

        public Tradable? GetTradableBySymbol(string symbol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol FROM Tradables WHERE symbol = @Symbol;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Symbol", symbol);

                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return _dataMapper.MapTradable(reader);
                    }
                }
            }

            return null;
        }
    }
}
