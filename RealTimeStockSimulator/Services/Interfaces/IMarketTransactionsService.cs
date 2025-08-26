using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IMarketTransactionsService
    {
        MarketTransactions GetTransactionsByUserPagnated(User user);
        int AddTransaction(User user, MarketTransactionTradable transaction);
    }
}
