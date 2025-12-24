using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class TradablesService : ITradablesService
    {
        private string? _marketApiKey;
        private ITradablePriceInfosService _priceInfosService;
        ITradablesRepository _tradablesRepository;

        public TradablesService(IConfiguration configuration, ITradablePriceInfosService priceInfosService, ITradablesRepository tradablesRepository)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _priceInfosService = priceInfosService;
            _tradablesRepository = tradablesRepository;
        }

        public List<Tradable> GetAllTradables()
        {
            List<Tradable> tradables = _tradablesRepository.GetAllTradables();

            foreach(Tradable tradable in tradables)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
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

            if (tradable != null)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
            }
            
            return tradable;
        }

        public Tradable GetTradableFromBuySellViewModel(ProcessBuySellViewModel confirmViewModel)
        {
            if (confirmViewModel.Symbol == null)
            {
                throw new Exception("Symbol is empty.");
            }

            Tradable? tradable = GetTradableBySymbol(confirmViewModel.Symbol);

            if (tradable == null)
            {
                throw new Exception("Symbol does not exist.");
            }

            if (tradable.TradablePriceInfos == null)
            {
                throw new Exception("Symbol does not have a price.");
            }

            return tradable;
        }
    }
}
