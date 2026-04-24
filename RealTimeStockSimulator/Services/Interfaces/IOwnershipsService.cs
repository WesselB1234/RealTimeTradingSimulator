using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IOwnershipsService
    {
        List<OwnershipAsset> GetAllOwnershipAssetsByUserId(int userId);
        MultiOwnership GetValueOrderedMultiOwnershipsPagnated(int pageSize, int currentPage);
        OwnershipAsset? GetOwnershipAssetByUserId(int userId, string symbol);
        OwnershipAsset GetOwnershipAssetFromSymbolAndUserIdOrThrow(string symbol, int userId);
        void AddOwnershipAssetToUserId(int userId, OwnershipAsset asset);
        void UpdateOwnershipAsset(int userId, OwnershipAsset asset);
        void RemoveOwnershipAssetFromUserId(int userId, OwnershipAsset asset);
        decimal BuyAsset(UserAccount user, Asset asset, int amount);
        decimal SellAsset(UserAccount user, OwnershipAsset asset, int amount);
    }
}
