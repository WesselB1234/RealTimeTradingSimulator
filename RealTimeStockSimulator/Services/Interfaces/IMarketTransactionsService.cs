using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IMarketTransactionsService
    {
        List<MarketTransactionAsset> GetTransactionsByUserPagnated(int userId, int pageSize, int currentPage);
        int AddTransaction(int userId, MarketTransactionAsset transaction);
    }
}
