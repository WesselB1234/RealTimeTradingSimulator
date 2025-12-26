using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IOwnershipsService
    {
        List<OwnershipTradable> GetAllOwnershipTradablesByUserId(int userId);
        MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipTradable? GetOwnershipTradableByUserId(int userId, string symbol);
        OwnershipTradable GetOwnershipTradableFromBuySellViewModel(ProcessBuySellVM confirmViewModel, int userId);
        void AddOwnershipTradableToUserId(int userId, OwnershipTradable tradable);
        void UpdateOwnershipTradable(int userId, OwnershipTradable tradable);
        void RemoveOwnershipTradableFromUserId(int userId, OwnershipTradable tradable);
        decimal BuyTradable(UserAccount user, Tradable tradable, int amount);
        decimal SellTradable(UserAccount user, OwnershipTradable tradable, int amount);
    }
}
