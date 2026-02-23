using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class CacheAssetPriceInfosRepository : IAssetPriceInfosRepository
    {
        private Dictionary<string, AssetPriceInfos> _assetsPriceInfosDictionary = new Dictionary<string, AssetPriceInfos>();

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            if (_assetsPriceInfosDictionary.ContainsKey(symbol))
            {
                return _assetsPriceInfosDictionary[symbol];
            }

            return null;
        }

        public List<string> GetAllKeys()
        {
            return _assetsPriceInfosDictionary.Keys.ToList();
        }

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _assetsPriceInfosDictionary[symbol] = priceInfos;
        }
    }
}
