using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IMarketTransactionsRepository
    {
        MarketTransactions GetTransactionsByUserPagnated(User user);
        int AddTransaction(User user, MarketTransactionTradable transaction);
    }
}
