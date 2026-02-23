using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class ApiCacheInitializer : IHostedService
    {
        private IAssetsPriceInfosService _priceInfosService;
        private IAssetsService _assetsService;

        public ApiCacheInitializer(IAssetsPriceInfosService priceInfosService, IAssetsService assetsService)
        {
            _priceInfosService = priceInfosService;
            _assetsService = assetsService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (Asset asset in await _assetsService.GetAllAssetsWithApiDataAsync(cancellationToken))
            {
                if (asset.AssetPriceInfos != null)
                {
                    _priceInfosService.SetPriceInfosBySymbol(asset.Symbol, asset.AssetPriceInfos);
                }
                else
                {
                    _priceInfosService.SetPriceInfosBySymbol(asset.Symbol, new AssetPriceInfos(0));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
