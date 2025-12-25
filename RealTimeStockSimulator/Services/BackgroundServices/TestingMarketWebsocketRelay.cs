using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;
using System.Threading.Tasks;

namespace RealTimeStockSimulator.Services.BackgroundServices
{
    public class TestingMarketWebsocketRelay : BackgroundService
    {   
        private ITradablePriceInfosService _priceInfosService;
        private IMarketWebsocketHandler _marketWebsocketHandler;
        private static Random _random = new Random();

        public TestingMarketWebsocketRelay(ITradablePriceInfosService priceInfosService, IMarketWebsocketHandler marketWebsocketHandler)
        {
            _priceInfosService = priceInfosService;
            _marketWebsocketHandler = marketWebsocketHandler;
        }

        private void SubscribeToTradablesInCache(CancellationToken cancellationToken)
        {
            foreach (string key in _priceInfosService.GetAllKeys())
            {
                Task task = Task.Run(async () => 
                { 
                    while (!cancellationToken.IsCancellationRequested) 
                    { 
                        Console.WriteLine(key); 
                        await Task.Delay(_random.Next(0,10000), cancellationToken);
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
