using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IOwnershipsService
    {
        List<OwnershipAsset> GetAllOwnershipTradablesByUserId(int userId);
        MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipAsset? GetOwnershipTradableByUserId(int userId, string symbol);
        OwnershipAsset GetOwnershipTradableFromBuySellViewModel(ProcessBuySellVM confirmViewModel, int userId);
        void AddOwnershipTradableToUserId(int userId, OwnershipAsset tradable);
        void UpdateOwnershipTradable(int userId, OwnershipAsset tradable);
        void RemoveOwnershipTradableFromUserId(int userId, OwnershipAsset tradable);
        decimal BuyTradable(UserAccount user, Asset tradable, int amount);
        decimal SellTradable(UserAccount user, OwnershipAsset tradable, int amount);
    }
}
