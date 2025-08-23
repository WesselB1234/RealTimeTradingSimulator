using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface ITradablesRepository
    {
        List<Tradable> GetAllTradables();
        Tradable? GetTradableBySymbol(string symbol);
    }
}
