using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.BackgroundServices
{
    public class TestingMarketWebsocketRelay : BackgroundService
    {   
        private ITradablePriceInfosService _priceInfosService;
        private IMarketWebsocketHandler _marketWebsocketHandler;

        public TestingMarketWebsocketRelay(ITradablePriceInfosService priceInfosService, IMarketWebsocketHandler marketWebsocketHandler)
        {
            _priceInfosService = priceInfosService;
            _marketWebsocketHandler = marketWebsocketHandler;
        }

        private void SubscribeToTradablesInCache()
        {
            foreach (string key in _priceInfosService.GetAllKeys())
            {
                Console.WriteLine(key);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            SubscribeToTradablesInCache();

            await Task.CompletedTask;
        }
    }
}
