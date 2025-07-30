
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class ApiCacheInitializer : IHostedService
    {
        private string? _marketApiKey;
        private IMemoryCache _memoryCache;
        private ITradablesService _tradablesService;

        public ApiCacheInitializer(IConfiguration configuration, IMemoryCache memoryCache, ITradablesService tradablesService)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _memoryCache = memoryCache;
            _tradablesService = tradablesService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _memoryCache.Set("TradablesDictionary", new Dictionary<string, Tradable>());

            foreach(Tradable tradable in _tradablesService.GetAllTradables())
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol=AAPL&token={_marketApiKey}", cancellationToken);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(content);
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Request error: {ex.Message}");
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
