using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class MarketTransactionsService : IMarketTransactionsService
    {
        private IMarketTransactionsRepository _marketTransactionsRepository;
        public MarketTransactionsService(IMarketTransactionsRepository marketTransactionsRepository)
        {
            _marketTransactionsRepository = marketTransactionsRepository;
        }

        public int AddTransaction(User user, MarketTransactionTradable transaction)
        {
            return _marketTransactionsRepository.AddTransaction(user ,transaction);
        }

        public MarketTransactions GetTransactionsByUserPagnated(User user)
        {
            return _marketTransactionsRepository.GetTransactionsByUserPagnated(user);
        }
    }
}
