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
        IAssetsRepository _assetsRepository;

        public AssetsService(IConfiguration configuration, IAssetsPriceInfosService priceInfosService, IAssetsRepository assetsRepository)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _priceInfosService = priceInfosService;
            _assetsRepository = assetsRepository;
        }

        public int AddAsset(Asset asset)
        {
            return _assetsRepository.AddAsset(asset);
        }

        public List<Asset> GetAllAssets()
        {
            List<Asset> assets = _assetsRepository.GetAllAssets();

            foreach(Asset asset in assets)
            {
                asset.AssetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(asset.Symbol);
            }

            return assets;
        }

        public async Task<List<Asset>> GetAllAssetsWithApiDataAsync(CancellationToken cancellationToken)
        {
            List<Asset> assets = GetAllAssets();
            HttpClient client = new HttpClient();

            foreach (Asset asset in assets) 
            {
                HttpResponseMessage response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol={asset.Symbol}&token={_marketApiKey}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    AssetPriceInfos? responseAssetPriceInfos = await response.Content.ReadFromJsonAsync<AssetPriceInfos>();

                    if (responseAssetPriceInfos != null)
                    {
                        asset.AssetPriceInfos = responseAssetPriceInfos;
                    }
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

            return assets;
        }

        public Asset? GetAssetBySymbol(string symbol)
        {
            Asset? asset = _assetsRepository.GetAssetBySymbol(symbol);

            if (asset != null)
            {
                asset.AssetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(asset.Symbol);
            }
            
            return asset;
        }

        public Asset GetAssetBySymbolOrThrow(string symbol)
        {
            Asset? asset = GetAssetBySymbol(symbol);

            if (asset == null)
            {
                throw new Exception("Symbol does not exist.");
            }

            if (asset.AssetPriceInfos == null)
            {
                throw new Exception("Symbol does not have a price.");
            }

            return asset;
        }
    }
}
