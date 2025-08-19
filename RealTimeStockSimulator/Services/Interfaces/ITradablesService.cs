using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Services.Interfaces
{
    public interface ITradablesService
    {
        public List<Tradable> GetAllTradables();
        public Task<List<Tradable>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken);
    }
}
