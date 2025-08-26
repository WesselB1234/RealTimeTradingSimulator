using Microsoft.Data.SqlClient;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbMarketTransactionsRepository : DbBaseRepository, IMarketTransactionsRepository
    {
        public DbMarketTransactionsRepository(IConfiguration configuration, IDataMapper dataMapper) : base(configuration, dataMapper) { }

        public int AddTransaction(User user, MarketTransactionTradable transaction)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Transactions(user_id, symbol, price, status, amount, date) " +
                    "VALUES (@UserId, @Symbol, @Price, @Status, @Amount, @Date)" +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
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

        public MarketTransactions GetTransactionsByUserPagnated(User user)
        {
            MarketTransactions transactions = new MarketTransactions(user, new List<MarketTransactionTradable>());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT TOP(25) transaction_id, symbol, price, status, amount, date " +
                   "FROM Transactions " +
                   "WHERE user_id = @UserId " +
                   "ORDER BY transaction_id DESC;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                     transactions.Transactions.Add(_dataMapper.MapMarketTransactionTradable(reader));
                }
            }

            return transactions;
        }
    }
}
