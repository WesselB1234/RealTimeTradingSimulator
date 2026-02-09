using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class ApiCacheInitializer : IHostedService
    {
        private IAssetsPriceInfosService _priceInfosService;
        private IAssetsService _tradablesService;

        public ApiCacheInitializer(IAssetsPriceInfosService priceInfosService, IAssetsService tradablesService)
        {
            _priceInfosService = priceInfosService;
            _tradablesService = tradablesService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (Asset tradable in await _tradablesService.GetAllTradablesWithApiDataAsync(cancellationToken))
            {
                if (tradable.TradablePriceInfos != null)
                {
                    _priceInfosService.SetPriceInfosBySymbol(tradable.Symbol, tradable.TradablePriceInfos);
                }
                else
                {
                    _priceInfosService.SetPriceInfosBySymbol(tradable.Symbol, new AssetPriceInfos(0));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
