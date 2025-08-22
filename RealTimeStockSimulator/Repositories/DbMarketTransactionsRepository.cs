using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbMarketTransactionsRepository : IMarketTransactionsRepository
    {
        public List<MarketTransaction> GetTransactionsByUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
