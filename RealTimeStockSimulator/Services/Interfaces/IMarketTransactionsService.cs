using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IMarketTransactionsService
    {
        List<MarketTransactionTradable> GetTransactionsByUserPagnated(int userId, int pageSize, int currentPage);
        int AddTransaction(int userId, MarketTransactionTradable transaction);
    }
}
