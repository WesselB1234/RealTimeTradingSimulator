using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class CacheAssetPriceInfosRepository : IAssetPriceInfosRepository
    {
        private Dictionary<string, AssetPriceInfos> _tradablesPriceInfosDictionary = new Dictionary<string, AssetPriceInfos>();

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
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

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _tradablesPriceInfosDictionary[symbol] = priceInfos;
        }
    }
}
