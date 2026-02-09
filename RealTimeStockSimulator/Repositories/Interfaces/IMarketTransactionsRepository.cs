using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IMarketTransactionsRepository
    {
        List<MarketTransactionAsset> GetTransactionsByUserIdPagnated(int userId, int pageSize, int currentPage);
        int AddTransaction(int userId, MarketTransactionAsset transaction);
    }
}
