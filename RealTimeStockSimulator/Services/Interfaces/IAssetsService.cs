using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IAssetsService
    {
        List<Asset> GetAllAssets();
        int AddAsset(Asset asset);
        Task<List<Asset>> GetAllAssetsWithApiDataAsync(CancellationToken cancellationToken);
        Asset GetAssetBySymbolOrThrow(string symbol);
        Asset? GetAssetBySymbol(string symbol);
    }
}
