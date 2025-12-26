using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IOwnershipsRepository
    {
        List<OwnershipTradable> GetAllOwnershipTradablesByUserId(int userId);
        List<Ownership> GetOrderedOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipTradable? GetOwnershipTradableByUserId(int userId, string symbol);
        void AddOwnershipTradableToUserId(int userId, OwnershipTradable tradable);
        void UpdateOwnershipTradable(int userId, OwnershipTradable tradable);
        void RemoveOwnershipTradableFromUserId(int userId, OwnershipTradable tradable);
    }
}
