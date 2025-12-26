using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.BackgroundServices
{
    public class TestingMarketWebsocketRelay : BackgroundService
    {   
        private ITradablePriceInfosService _priceInfosService;
        private IMarketWebsocketHandler _marketWebsocketHandler;
        private Random _random = new Random();

        public TestingMarketWebsocketRelay(ITradablePriceInfosService priceInfosService, IMarketWebsocketHandler marketWebsocketHandler)
        {
            _priceInfosService = priceInfosService;
            _marketWebsocketHandler = marketWebsocketHandler;
        }

        private decimal AddRandomnessToPrice(decimal currentPrice)
        {
            double offsetPercentage = (double)_random.Next(-33, 33) / 10; 
            currentPrice = currentPrice * (decimal)(1 + (offsetPercentage / 100));

            if (currentPrice < 1)
            {
                currentPrice = 1;
            }

            return currentPrice;
        }

        private void SubscribeToTradablesInCache(CancellationToken cancellationToken)
        {
            foreach (string symbol in _priceInfosService.GetAllKeys())
            {
                Task task = Task.Run(async () => 
                { 
                    while (!cancellationToken.IsCancellationRequested) 
                    { 
                        TradablePriceInfos currentPriceInfos = _priceInfosService.GetPriceInfosBySymbol(symbol);
                        IncomingMarketWebsocketTradable incomingMarketWebsocketTradable = new IncomingMarketWebsocketTradable(symbol, AddRandomnessToPrice(currentPriceInfos.Price));

                        await _marketWebsocketHandler.HandleMarketWebSocketPayload(incomingMarketWebsocketTradable, cancellationToken);
                        await Task.Delay(_random.Next(0, 10000), cancellationToken);
                    } 
                }, cancellationToken);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            SubscribeToTradablesInCache(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
