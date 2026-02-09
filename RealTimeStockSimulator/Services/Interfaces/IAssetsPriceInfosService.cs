using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IAssetsPriceInfosService
    {
        AssetPriceInfos? GetPriceInfosBySymbol(string symbol);
        List<string> GetAllKeys();
        void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos);
    }
}
