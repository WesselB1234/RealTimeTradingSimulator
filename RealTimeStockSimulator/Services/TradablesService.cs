using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class TradablesService : ITradablesService
    {
        private string? _marketApiKey;
        IMemoryCache _memoryCache;
        ITradablesRepository _tradablesRepository;

        public TradablesService(IConfiguration configuration, IMemoryCache memoryCache, ITradablesRepository tradablesRepository)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _memoryCache = memoryCache;
            _tradablesRepository = tradablesRepository;
        }

        public List<Tradable> GetAllTradables()
        {
            List<Tradable> tradables = _tradablesRepository.GetAllTradables();
            Dictionary<string, TradablePriceInfos>? tradablePriceInfosDictionary = _memoryCache.Get<Dictionary<string, TradablePriceInfos>?>("TradablePriceInfosDictionary");

            if (tradablePriceInfosDictionary != null)
            {
                foreach(Tradable tradable in tradables)
                {
                    tradable.TradablePriceInfos = tradablePriceInfosDictionary[tradable.Symbol];
                }
            }

            return tradables;
        }

        public async Task<List<Tradable>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken)
        {
            List<Tradable> tradables = GetAllTradables();
            HttpClient client = new HttpClient();

            foreach (Tradable tradable in tradables) 
            {
                HttpResponseMessage response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol={tradable.Symbol}&token={_marketApiKey}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    TradablePriceInfos? responseTradablePriceInfos = await response.Content.ReadFromJsonAsync<TradablePriceInfos>();

                    if (responseTradablePriceInfos != null)
                    {
                        tradable.TradablePriceInfos = responseTradablePriceInfos;
                    }
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

            return tradables;
        }

        public Tradable? GetTradableBySymbol(string symbol)
        {
            Tradable? tradable = _tradablesRepository.GetTradableBySymbol(symbol);
            Dictionary<string, TradablePriceInfos>? tradablePriceInfosDictionary = _memoryCache.Get<Dictionary<string, TradablePriceInfos>?>("TradablePriceInfosDictionary");

            if (tradablePriceInfosDictionary != null && tradable != null)
            {
                tradable.TradablePriceInfos = tradablePriceInfosDictionary[tradable.Symbol];
            }

            return tradable;
        }
    }
}
