using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class CacheTradablePriceInfosRepository : ITradablePriceInfosRepository
    {
        private Dictionary<string, TradablePriceInfos> _tradablesPriceInfosDictionary = new Dictionary<string, TradablePriceInfos>();

        public TradablePriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            return _tradablesPriceInfosDictionary[symbol];
        }

        public Dictionary<string, TradablePriceInfos> GetPriceInfosDictionary()
        {
            return _tradablesPriceInfosDictionary;
        }

        public void SetPriceInfosBySymbol(string symbol, TradablePriceInfos priceInfos)
        {
            _tradablesPriceInfosDictionary.Add(symbol, priceInfos);
        }
    }
}
