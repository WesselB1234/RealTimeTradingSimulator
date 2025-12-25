using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Enums;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class OwnershipsService : IOwnershipsService
    {
        private IOwnershipsRepository _ownershipsRepository;
        private IMarketTransactionsService _marketTransactionsService;
        private ITradablePriceInfosService _priceInfosService;

        public OwnershipsService(IOwnershipsRepository ownershipsRepository, IMarketTransactionsService marketTransactionsService, ITradablePriceInfosService priceInfosService)
        {
            _ownershipsRepository = ownershipsRepository;
            _marketTransactionsService = marketTransactionsService;
            _priceInfosService = priceInfosService;
        }

        public List<OwnershipTradable> GetAllOwnershipTradablesByUserId(int userId)
        {
            List<OwnershipTradable> ownershipTradables = _ownershipsRepository.GetAllOwnershipTradablesByUserId(userId);
          
            foreach (OwnershipTradable tradable in ownershipTradables)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
            }

            return ownershipTradables;
        }

        public OwnershipTradable? GetOwnershipTradableByUserId(int userId, string symbol)
        {
            OwnershipTradable? tradable = _ownershipsRepository.GetOwnershipTradableByUserId(userId, symbol);
          
            if (tradable != null)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(symbol);
            }

            return tradable;
        }

        public void AddOwnershipTradableToUserId(int userId, OwnershipTradable tradable)
        {
            _ownershipsRepository.AddOwnershipTradableToUserId(userId, tradable);
        }

        public void UpdateOwnershipTradable(int userId, OwnershipTradable tradable)
        {
            _ownershipsRepository.UpdateOwnershipTradable(userId, tradable);
        }

        public void RemoveOwnershipTradableFromUserId(int userId, OwnershipTradable tradable)
        {
            _ownershipsRepository.RemoveOwnershipTradableFromUserId(userId, tradable);
        }

        private void LogOrderTransaction(int userId, Tradable tradable, MarketTransactionStatus status, int amount)
        {
            MarketTransactionTradable marketTransactionTradable = new MarketTransactionTradable(tradable, tradable.TradablePriceInfos.Price, status, amount, DateTime.Now);

            _marketTransactionsService.AddTransaction(userId, marketTransactionTradable);
        }

        public decimal BuyTradable(UserAccount user, Tradable tradable, int amount)
        {
            decimal moneyAfterPurchase = user.Money - (tradable.TradablePriceInfos.Price * amount);

            if (moneyAfterPurchase < 0)
            {
                throw new ArgumentException("You do not have enough money for this order.");
            }

            OwnershipTradable? ownershipTradable = _ownershipsRepository.GetOwnershipTradableByUserId(user.UserId, tradable.Symbol);

            if (ownershipTradable != null)
            {
                ownershipTradable.Amount += amount;
                UpdateOwnershipTradable(user.UserId, ownershipTradable);
            }
            else
            {
                AddOwnershipTradableToUserId(user.UserId, DataMapper.MapOwnershipTradableByTradable(tradable, amount));
            }

            LogOrderTransaction(user.UserId, tradable, MarketTransactionStatus.Bought, amount);

            return moneyAfterPurchase;
        }

        public decimal SellTradable(UserAccount user, OwnershipTradable tradable, int amount)
        {
            OwnershipTradable? ownershipTradable = _ownershipsRepository.GetOwnershipTradableByUserId(user.UserId, tradable.Symbol);

            if (amount > tradable.Amount)
            {
                throw new ArgumentException("You do not own this amount.");
            }

            if (ownershipTradable != null && ownershipTradable.Amount - amount >= 1)
            {
                ownershipTradable.Amount -= amount;
                UpdateOwnershipTradable(user.UserId, ownershipTradable);
            }
            else
            {
                RemoveOwnershipTradableFromUserId(user.UserId, tradable);
            }

            LogOrderTransaction(user.UserId, tradable, MarketTransactionStatus.Sold, amount);

            return user.Money + (tradable.TradablePriceInfos.Price * amount);
        }

        public OwnershipTradable GetOwnershipTradableFromBuySellViewModel(ProcessBuySellViewModel confirmViewModel, int userId)
        {
            if (confirmViewModel.Symbol == null)
            {
                throw new Exception("Symbol is empty.");
            }

            OwnershipTradable? tradable = GetOwnershipTradableByUserId(userId, confirmViewModel.Symbol);

            if (tradable == null)
            {
                throw new Exception("Symbol does not exist or you do not own this symbol.");
            }

            if (tradable.TradablePriceInfos == null)
            {
                throw new Exception("Symbol does not have a price.");
            }

            return tradable;
        }

        public List<Ownership> GetOrderedOwnershipsPagnated(int userId, int pageSize, int currentPage)
        {
            throw new NotImplementedException();
        }
    }
}
