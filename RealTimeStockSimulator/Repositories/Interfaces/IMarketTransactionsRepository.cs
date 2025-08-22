using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IMarketTransactionsRepository
    {
        List<MarketTransaction> GetTransactionsByUser(User user);
    }
}
