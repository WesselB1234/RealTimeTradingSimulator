using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class TestingCacheInitializer : IHostedService
    {
        private IAssetsPriceInfosService _priceInfosService;
        private IAssetsService _assetsService;
        private Random _random = new Random();

        public TestingCacheInitializer(IAssetsPriceInfosService priceInfosService, IAssetsService assetsService)
        {
            _priceInfosService = priceInfosService;
            _assetsService = assetsService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (Asset tradable in _assetsService.GetAllAssets())
            {
                _priceInfosService.SetPriceInfosBySymbol(tradable.Symbol, new AssetPriceInfos(_random.Next(1,10000)));
            }

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
