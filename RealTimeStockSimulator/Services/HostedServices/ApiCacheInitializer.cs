
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class ApiCacheInitializer : IHostedService
    {
        private IMemoryCache _memoryCache;
        private ITradablesService _tradablesService;

        public ApiCacheInitializer(IMemoryCache memoryCache, ITradablesService tradablesService)
        {
            _memoryCache = memoryCache;
            _tradablesService = tradablesService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Dictionary<string, Tradable> tradablesDictionary = new Dictionary<string, Tradable>();

            foreach (Tradable tradable in await _tradablesService.GetAllTradablesWithApiDataAsync(cancellationToken))
            {
                tradablesDictionary.Add(tradable.Symbol, tradable);
            }

            _memoryCache.Set("TradablesDictionary", tradablesDictionary);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
