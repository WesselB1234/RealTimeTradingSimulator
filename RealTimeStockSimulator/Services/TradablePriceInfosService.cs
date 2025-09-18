using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class TradablePriceInfosService : ITradablePriceInfosService
    {
        private ITradablePriceInfosRepository _tradablePriceInfosRepository;

        public TradablePriceInfosService(ITradablePriceInfosRepository tradablePriceInfosRepository)
        {
            _tradablePriceInfosRepository = tradablePriceInfosRepository;
        }

        public TradablePriceInfos? GetPriceInfosBySymbol(string symbol)
        {
            return _tradablePriceInfosRepository.GetPriceInfosBySymbol(symbol);
        }

        public Dictionary<string, TradablePriceInfos> GetPriceInfosDictionary()
        {
            return _tradablePriceInfosRepository.GetPriceInfosDictionary();
        }

        public void SetPriceInfosBySymbol(string symbol, TradablePriceInfos priceInfos)
        {
            _tradablePriceInfosRepository.SetPriceInfosBySymbol(symbol, priceInfos);
        }
    }
}
