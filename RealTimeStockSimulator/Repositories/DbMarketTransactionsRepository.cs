using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbMarketTransactionsRepository : DbBaseRepository, IMarketTransactionsRepository
    {
        public DbMarketTransactionsRepository(IConfiguration configuration) : base(configuration) { }

        public int AddTransaction(int userId, MarketTransactionTradable transaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Transactions(user_id, symbol, price, status, amount, date) " +
                    "VALUES (@UserId, @Symbol, @Price, @Status, @Amount, @Date)" +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Symbol", transaction.Tradable.Symbol);
                command.Parameters.AddWithValue("@Price", transaction.Price);
                command.Parameters.AddWithValue("@Status", transaction.Status.ToString());
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Connection.Open();

                int? transactionId = Convert.ToInt32(command.ExecuteScalar());

                if (transactionId == null)
                {
                    throw new Exception("Insert transaction did not return a valid transaction_id.");
                }

                return (int)transactionId;
            }
        }

        public List<MarketTransactionTradable> GetTransactionsByUserIdPagnated(int userId, int pageSize, int currentPage)
        {
            List<MarketTransactionTradable> transactions = new List<MarketTransactionTradable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT TOP(@PageSize) transaction_id, symbol, price, status, amount, date " +
                   "FROM Transactions " +
                   "WHERE user_id = @UserId " +
                   "ORDER BY transaction_id DESC;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                     transactions.Add(DataMapper.MapMarketTransactionTradable(reader));
                }
            }

            return transactions;
        }
    }
}
