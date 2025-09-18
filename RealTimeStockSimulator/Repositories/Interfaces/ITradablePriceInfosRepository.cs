using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface ITradablePriceInfosRepository
    {
        TradablePriceInfos? GetPriceInfosBySymbol(string symbol);
        Dictionary<string, TradablePriceInfos> GetPriceInfosDictionary();
        void SetPriceInfosBySymbol(string symbol, TradablePriceInfos priceInfos);
    }
}
