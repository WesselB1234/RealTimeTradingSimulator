using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class AssetsService : IAssetsService
    {
        private string? _marketApiKey;
        private IAssetsPriceInfosService _priceInfosService;
        IAssetsRepository _tradablesRepository;

        public AssetsService(IConfiguration configuration, IAssetsPriceInfosService priceInfosService, IAssetsRepository tradablesRepository)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _priceInfosService = priceInfosService;
            _tradablesRepository = tradablesRepository;
        }

        public int AddTradable(Asset tradable)
        {
            return _tradablesRepository.AddTradable(tradable);
        }

        public List<Asset> GetAllTradables()
        {
            List<Asset> tradables = _tradablesRepository.GetAllTradables();

            foreach(Asset tradable in tradables)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
            }

            return tradables;
        }

        public async Task<List<Asset>> GetAllTradablesWithApiDataAsync(CancellationToken cancellationToken)
        {
            List<Asset> tradables = GetAllTradables();
            HttpClient client = new HttpClient();

            foreach (Asset tradable in tradables) 
            {
                HttpResponseMessage response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol={tradable.Symbol}&token={_marketApiKey}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    AssetPriceInfos? responseTradablePriceInfos = await response.Content.ReadFromJsonAsync<AssetPriceInfos>();

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

        public Asset? GetTradableBySymbol(string symbol)
        {
            Asset? tradable = _tradablesRepository.GetTradableBySymbol(symbol);

            if (tradable != null)
            {
                tradable.TradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(tradable.Symbol);
            }
            
            return tradable;
        }

        public Asset GetTradableFromBuySellViewModel(ProcessBuySellVM confirmViewModel)
        {
            if (confirmViewModel.Symbol == null)
            {
                throw new Exception("Symbol is empty.");
            }

            Asset? tradable = GetTradableBySymbol(confirmViewModel.Symbol);

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
