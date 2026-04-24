using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Enums;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class OwnershipsService : IOwnershipsService
    {
        private IOwnershipRepository _ownershipsRepository;
        private IMarketTransactionsService _marketTransactionsService;
        private IAssetsPriceInfosService _priceInfosService;

        public OwnershipsService(IOwnershipRepository ownershipsRepository, IMarketTransactionsService marketTransactionsService, IAssetsPriceInfosService priceInfosService)
        {
            _ownershipsRepository = ownershipsRepository;
            _marketTransactionsService = marketTransactionsService;
            _priceInfosService = priceInfosService;
        }

        public List<OwnershipAsset> GetAllOwnershipAssetsByUserId(int userId)
        {
            List<OwnershipAsset> ownershipAssets = _ownershipsRepository.GetAllOwnershipAssetsByUserId(userId);
          
            foreach (OwnershipAsset ownershipAsset in ownershipAssets)
            {
                ownershipAsset.AssetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(ownershipAsset.Symbol);
            }

            return ownershipAssets;
        }

        public OwnershipAsset? GetOwnershipAssetByUserId(int userId, string symbol)
        {
            OwnershipAsset? ownershipAssets = _ownershipsRepository.GetOwnershipAssetByUserId(userId, symbol);
          
            if (ownershipAssets != null)
            {
                ownershipAssets.AssetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(symbol);
            }

            return ownershipAssets;
        }

        public void AddOwnershipAssetToUserId(int userId, OwnershipAsset ownershipAssets)
        {
            _ownershipsRepository.AddOwnershipAssetToUserId(userId, ownershipAssets);
        }

        public void UpdateOwnershipAsset(int userId, OwnershipAsset ownershipAssets)
        {
            _ownershipsRepository.UpdateOwnershipAsset(userId, ownershipAssets);
        }

        public void RemoveOwnershipAssetFromUserId(int userId, OwnershipAsset ownershipAssets)
        {
            _ownershipsRepository.RemoveOwnershipAssetFromUserId(userId, ownershipAssets);
        }

        private void LogOrderTransaction(int userId, Asset asset, MarketTransactionStatus status, int amount)
        {
            MarketTransactionAsset marketTransactionAsset = new MarketTransactionAsset(asset, asset.AssetPriceInfos.Price, status, amount, DateTime.Now);

            _marketTransactionsService.AddTransaction(userId, marketTransactionAsset);
        }

        public decimal BuyAsset(UserAccount user, Asset asset, int amount)
        {
            decimal moneyAfterPurchase = user.Money - (asset.AssetPriceInfos.Price * amount);

            if (moneyAfterPurchase < 0)
            {
                throw new ArgumentException("You do not have enough money for this order.");
            }

            OwnershipAsset? ownershipAsset = _ownershipsRepository.GetOwnershipAssetByUserId(user.UserId, asset.Symbol);

            if (ownershipAsset != null)
            {
                ownershipAsset.Amount += amount;
                UpdateOwnershipAsset(user.UserId, ownershipAsset);
            }
            else
            {
                AddOwnershipAssetToUserId(user.UserId, DataMapper.MapOwnershipAssetByAsset(asset, amount));
            }

            LogOrderTransaction(user.UserId, asset, MarketTransactionStatus.Bought, amount);

            return moneyAfterPurchase;
        }

        public decimal SellAsset(UserAccount user, OwnershipAsset ownershipAsset, int amount)
        {
            if (amount > ownershipAsset.Amount)
            {
                throw new ArgumentException("You do not own this amount.");
            }

            if (ownershipAsset.Amount - amount >= 1)
            {
                ownershipAsset.Amount -= amount;
                UpdateOwnershipAsset(user.UserId, ownershipAsset);
            }
            else
            {
                RemoveOwnershipAssetFromUserId(user.UserId, ownershipAsset);
            }

            LogOrderTransaction(user.UserId, ownershipAsset, MarketTransactionStatus.Sold, amount);

            return user.Money + (ownershipAsset.AssetPriceInfos.Price * amount);
        }

        public OwnershipAsset GetOwnershipAssetFromSymbolAndUserIdOrThrow(string symbol, int userId)
        {
            OwnershipAsset? ownershipAsset = GetOwnershipAssetByUserId(userId, symbol);

            if (ownershipAsset == null)
            {
                throw new Exception("Asset does not exist or you do not own this asset.");
            }

            if (ownershipAsset.AssetPriceInfos == null)
            {
                throw new Exception("Asset does not have a price.");
            }

            return ownershipAsset;
        }

        public MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage)
        {
            MultiOwnership multiOwnership = _ownershipsRepository.GetValueOrderedMultiOwnershipsPagnated(pageSize, currentPage);

            foreach (Asset asset in multiOwnership.AssetsDictionary.Values)
            {
                asset.AssetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(asset.Symbol);   
            }

            multiOwnership.Ownerships = multiOwnership.Ownerships.OrderByDescending(o => o.GetTotalValue(multiOwnership.AssetsDictionary)).ToList();

            return multiOwnership;
        }
    }
}
