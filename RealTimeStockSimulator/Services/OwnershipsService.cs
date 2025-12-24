using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Enums;
using RealTimeStockSimulator.Models.Interfaces;
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
        private IDataMapper _mapper;

        public OwnershipsService(IOwnershipsRepository ownershipsRepository, IMarketTransactionsService marketTransactionsService, ITradablePriceInfosService priceInfosService, IDataMapper mapper)
        {
            _ownershipsRepository = ownershipsRepository;
            _marketTransactionsService = marketTransactionsService;
            _priceInfosService = priceInfosService;
            _mapper = mapper;
        }

        public Ownership GetOwnershipByUser(UserAccount user)
        {
            Ownership ownership = _ownershipsRepository.GetOwnershipByUser(user);
          
            foreach (OwnershipTradable tradable in ownership.Tradables)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
            }

            return ownership;
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

        public void AddOwnershipTradableToUser(UserAccount user, OwnershipTradable tradable)
        {
            _ownershipsRepository.AddOwnershipTradableToUser(user, tradable);
        }

        public void UpdateOwnershipTradable(UserAccount user, OwnershipTradable tradable)
        {
            _ownershipsRepository.UpdateOwnershipTradable(user, tradable);
        }

        public void RemoveOwnershipTradableFromUser(UserAccount user, OwnershipTradable tradable)
        {
            _ownershipsRepository.RemoveOwnershipTradableFromUser(user, tradable);
        }

        private void LogOrderTransaction(UserAccount user, Tradable tradable, MarketTransactionStatus status, int amount)
        {
            MarketTransactionTradable marketTransactionTradable = new MarketTransactionTradable(tradable, tradable.TradablePriceInfos.Price, status, amount, DateTime.Now);

            _marketTransactionsService.AddTransaction(user, marketTransactionTradable);
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
                UpdateOwnershipTradable(user, ownershipTradable);
            }
            else
            {
                AddOwnershipTradableToUser(user, _mapper.MapOwnershipTradableByTradable(tradable, amount));
            }

            LogOrderTransaction(user, tradable, MarketTransactionStatus.Bought, amount);

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
                UpdateOwnershipTradable(user, ownershipTradable);
            }
            else
            {
                RemoveOwnershipTradableFromUser(user, tradable);
            }

            LogOrderTransaction(user, tradable, MarketTransactionStatus.Sold, amount);

            return user.Money + (tradable.TradablePriceInfos.Price * amount);
        }

        public OwnershipTradable GetOwnershipTradableFromBuySellViewModel(ConfirmBuySellViewModel confirmViewModel, int userId)
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
    }
}
