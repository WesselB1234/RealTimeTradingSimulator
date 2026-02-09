using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IAssetsService
    {
        List<Asset> GetAllAssets();
        int AddAsset(Asset asset);
        Task<List<Asset>> GetAllAssersWithApiDataAsync(CancellationToken cancellationToken);
        Asset GetTradableFromBuySellViewModel(ProcessBuySellVM confirmViewModel);
        Asset? GetAssetBySymbol(string symbol);
    }
}
