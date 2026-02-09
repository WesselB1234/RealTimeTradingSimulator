using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IOwnershipRepository
    {
        List<OwnershipAsset> GetAllOwnershipTradablesByUserId(int userId);
        MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipAsset? GetOwnershipTradableByUserId(int userId, string symbol);
        void AddOwnershipTradableToUserId(int userId, OwnershipAsset tradable);
        void UpdateOwnershipTradable(int userId, OwnershipAsset tradable);
        void RemoveOwnershipTradableFromUserId(int userId, OwnershipAsset tradable);
    }
}
