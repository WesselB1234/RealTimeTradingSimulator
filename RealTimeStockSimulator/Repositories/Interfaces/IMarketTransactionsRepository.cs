using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IMarketTransactionsRepository
    {
        List<MarketTransactionTradable> GetTransactionsByUserIdPagnated(int userId, int pageSize, int currentPage);
        int AddTransaction(int userId, MarketTransactionTradable transaction);
    }
}
