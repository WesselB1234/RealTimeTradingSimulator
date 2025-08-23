using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface ITradablesService
    {
        List<Tradable> GetAllTradables();
        Task<List<Tradable>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken);
        Tradable? GetTradableBySymbol(string symbol);
    }
}
