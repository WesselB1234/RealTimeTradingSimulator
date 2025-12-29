using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models.Helpers;
using System.Data;

namespace RealTimeStockSimulator.Repositories
{
    public class DbTradablesRepository : DbBaseRepository, ITradablesRepository
    {
        public DbTradablesRepository(IConfiguration configuration) : base(configuration) { }

        public int AddTradable(Tradable tradable)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Tradables(symbol, name, image) " +
                    $"VALUES (@Symbol, @Name, @Image);" +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Symbol", tradable.Symbol);
                command.Parameters.AddWithValue("@Name", (tradable.Name == null ? DBNull.Value : tradable.Name));
                command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = (tradable.Image == null ? DBNull.Value : tradable.Image);

                command.Connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public List<Tradable> GetAllTradables()
        {
            List<Tradable> tradables = new List<Tradable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, name, image FROM Tradables";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Tradable tradable = DataMapper.MapTradable(reader);
                    tradables.Add(tradable);
                }
            }

            return tradables;
        }

        public Tradable? GetTradableBySymbol(string symbol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT symbol, name, image FROM Tradables WHERE symbol = @Symbol;";
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
