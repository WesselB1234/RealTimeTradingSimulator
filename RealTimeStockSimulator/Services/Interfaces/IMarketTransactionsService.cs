using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IMarketTransactionsService
    {
        List<MarketTransaction> GetTransactionsByUser(User user);
    }
}
