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

        public int AddTransaction(int userId, MarketTransactionAsset transaction)
        {
            return _marketTransactionsRepository.AddTransaction(userId, transaction);
        }

        public List<MarketTransactionAsset> GetTransactionsByUserPagnated(int userId, int pageSize, int currentPage)
        {
            return _marketTransactionsRepository.GetTransactionsByUserIdPagnated(userId, pageSize, currentPage);
        }
    }
}
