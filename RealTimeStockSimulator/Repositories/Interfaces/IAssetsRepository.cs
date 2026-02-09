using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IAssetsRepository
    {
        List<Asset> GetAllAssets();
        int AddAsset(Asset asset);
        Asset? GetAssetBySymbol(string symbol);
    }
}
