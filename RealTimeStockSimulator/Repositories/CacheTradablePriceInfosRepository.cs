using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class CacheTradablePriceInfosRepository : ITradablePriceInfosRepository
    {
        private Dictionary<string, TradablePriceInfos> _tradablesPriceInfosDictionary = new Dictionary<string, TradablePriceInfos>();

        public TradablePriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            if (_tradablesPriceInfosDictionary.ContainsKey(symbol))
            {
                return _tradablesPriceInfosDictionary[symbol];
            }

            return null;
        }

        public List<string> GetAllKeys()
        {
            return _tradablesPriceInfosDictionary.Keys.ToList();
        }

        public void SetPriceInfosBySymbol(string symbol, TradablePriceInfos priceInfos)
        {
            _tradablesPriceInfosDictionary[symbol] = priceInfos;
        }
    }
}
