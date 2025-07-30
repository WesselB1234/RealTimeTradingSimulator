using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class TradablesService : ITradablesService
    {
        private string? _marketApiKey;
        ITradablesRepository _tradablesRepository;

        public TradablesService(IConfiguration configuration, IMemoryCache memoryCache, ITradablesRepository tradablesRepository)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _tradablesRepository = tradablesRepository;
        }

        public List<Tradable> GetAllTradablesFromDb()
        {
            return _tradablesRepository.GetAllTradables();
        }

        public async Task<List<Tradable>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken)
        {
            List<Tradable> tradables = GetAllTradablesFromDb();
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    foreach (Tradable tradable in tradables) 
                    {
                        HttpResponseMessage response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol={tradable.Symbol}&token={_marketApiKey}", cancellationToken);

                        if (response.IsSuccessStatusCode)
                        {
                            Tradable? responseTradable = await response.Content.ReadFromJsonAsync<Tradable>();

                            if (responseTradable != null && responseTradable.Price != null)
                            {
                                tradable.Price = responseTradable.Price;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }
            }

            return tradables;
        }
    }
}
