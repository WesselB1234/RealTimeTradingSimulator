using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface ITradablesService
    {
        List<Tradable> GetAllTradables();
        Task<List<Tradable>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken);
        Tradable GetTradableFromBuySellViewModel(ProcessBuySellViewModel confirmViewModel);
        Tradable? GetTradableBySymbol(string symbol);
    }
}
