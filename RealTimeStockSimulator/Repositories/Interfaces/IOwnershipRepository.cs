using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IOwnershipRepository
    {
        List<OwnershipAsset> GetAllOwnershipAssetsByUserId(int userId);
        MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipAsset? GetOwnershipAssetByUserId(int userId, string symbol);
        void AddOwnershipAssetToUserId(int userId, OwnershipAsset asset);
        void UpdateOwnershipAsset(int userId, OwnershipAsset asset);
        void RemoveOwnershipAssetFromUserId(int userId, OwnershipAsset asset);
    }
}
