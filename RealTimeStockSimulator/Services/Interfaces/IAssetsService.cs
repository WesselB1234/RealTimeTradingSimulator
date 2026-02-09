using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface IAssetsService
    {
        List<Asset> GetAllTradables();
        int AddTradable(Asset tradable);
        Task<List<Asset>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken);
        Asset GetTradableFromBuySellViewModel(ProcessBuySellVM confirmViewModel);
        Asset? GetTradableBySymbol(string symbol);
    }
}
