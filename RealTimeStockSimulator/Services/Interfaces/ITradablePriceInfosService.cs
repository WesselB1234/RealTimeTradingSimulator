using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface ITradablePriceInfosService
    {
        TradablePriceInfos? GetPriceInfosBySymbol(string symbol);
        Dictionary<string, TradablePriceInfos> GetPriceInfosDictionary();
        void SetPriceInfosBySymbol(string symbol, TradablePriceInfos priceInfos);
    }
}
