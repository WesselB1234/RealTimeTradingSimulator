using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class ApiCacheInitializer : IHostedService
    {
        private ITradablePriceInfosService _priceInfosService;
        private ITradablesService _tradablesService;

        public ApiCacheInitializer(ITradablePriceInfosService priceInfosService, ITradablesService tradablesService)
        {
            _priceInfosService = priceInfosService;
            _tradablesService = tradablesService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (Tradable tradable in await _tradablesService.GetAllTradablesWithApiDataAsync(cancellationToken))
            {
                if (tradable.TradablePriceInfos != null)
                {
                    _priceInfosService.SetPriceInfosBySymbol(tradable.Symbol, tradable.TradablePriceInfos);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
