using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class AssetPriceInfosService : IAssetsPriceInfosService
    {
        private IAssetPriceInfosRepository _tradablePriceInfosRepository;

        public AssetPriceInfosService(IAssetPriceInfosRepository tradablePriceInfosRepository)
        {
            _tradablePriceInfosRepository = tradablePriceInfosRepository;
        }

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            return _tradablePriceInfosRepository.GetPriceInfosBySymbol(symbol);
        }

        public List<string> GetAllKeys()
        {
            return _tradablePriceInfosRepository.GetAllKeys();
        }

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _tradablePriceInfosRepository.SetPriceInfosBySymbol(symbol, priceInfos);
        }
    }
}
