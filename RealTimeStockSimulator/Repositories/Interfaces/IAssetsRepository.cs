using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IAssetsRepository
    {
        List<Asset> GetAllTradables();
        int AddTradable(Asset tradable);
        Asset? GetTradableBySymbol(string symbol);
    }
}
