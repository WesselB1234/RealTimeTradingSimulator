using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IAssetsService
    {
        List<Asset> GetAllAssets();
        int AddAsset(Asset asset);
        Task<List<Asset>> GetAllAssetsWithApiDataAsync(CancellationToken cancellationToken);
        Asset GetAssetFromBuySellViewModel(ProcessBuySellVM confirmViewModel);
        Asset? GetAssetBySymbol(string symbol);
    }
}
