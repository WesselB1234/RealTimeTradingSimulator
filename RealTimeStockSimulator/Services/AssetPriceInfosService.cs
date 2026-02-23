using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class AssetPriceInfosService : IAssetsPriceInfosService
    {
        private IAssetPriceInfosRepository _assetPriceInfosRepository;

        public AssetPriceInfosService(IAssetPriceInfosRepository assetPriceInfosRepository)
        {
            _assetPriceInfosRepository = assetPriceInfosRepository;
        }

        public AssetPriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            return _assetPriceInfosRepository.GetPriceInfosBySymbol(symbol);
        }

        public List<string> GetAllKeys()
        {
            return _assetPriceInfosRepository.GetAllKeys();
        }

        public void SetPriceInfosBySymbol(string symbol, AssetPriceInfos priceInfos)
        {
            _assetPriceInfosRepository.SetPriceInfosBySymbol(symbol, priceInfos);
        }
    }
}
