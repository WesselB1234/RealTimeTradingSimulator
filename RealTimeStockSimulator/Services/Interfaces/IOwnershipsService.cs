using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IOwnershipsService
    {
        Ownership GetOwnershipByUser(UserAccount user);
        OwnershipTradable? GetOwnershipTradableByUserId(int userId, string symbol);
        OwnershipTradable GetOwnershipTradableFromBuySellViewModel(ProcessBuySellViewModel confirmViewModel, int userId);
        void AddOwnershipTradableToUser(UserAccount user, OwnershipTradable tradable);
        void UpdateOwnershipTradable(UserAccount user, OwnershipTradable tradable);
        void RemoveOwnershipTradableFromUser(UserAccount user, OwnershipTradable tradable);
        decimal BuyTradable(UserAccount user, Tradable tradable, int amount);
        decimal SellTradable(UserAccount user, OwnershipTradable tradable, int amount);
    }
}
