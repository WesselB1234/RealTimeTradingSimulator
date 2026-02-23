using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Helpers;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services.HostedServices
{
    public class TestingCacheInitializer : IHostedService
    {
        private IAssetsPriceInfosService _priceInfosService;
        private IAssetsService _assetsService;
        private Random _random = new Random();

        public TestingCacheInitializer(IAssetsPriceInfosService priceInfosService, IAssetsService assetsService)
        {
            _priceInfosService = priceInfosService;
            _assetsService = assetsService;
        } 

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("AAPL", "Apple", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\AAPL.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("BINANCE:BTCUSDT", "Bitcoin", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\BTC.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("BINANCE:DOGEUSDT", "Dogecoin", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\DogeCoin.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("BINANCE:ETHUSDT", "Ethereum", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Ethereum_Logo.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("MCD", "McDonald's", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Mcdonalds.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("MSFT", "Microsoft", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Microsoft_logo.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("NKE", "Nike", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Nike.jpg")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("NVDA", "Nvidia", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Nividia.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("RBLX", "Roblox", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\Rblx.jpg")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("ASML", "ASML", ImageEncoder.GetBytesFromImagePath("C:\\Users\\stosi\\Desktop\\RealTimeTradingSimulator\\ASML.png")))
            //);

            //Console.WriteLine(
            //    _tradablesService.AddTradable(new Tradable("TEST", null, null))
            //);

            foreach (Asset tradable in _assetsService.GetAllAssets())
            {
                _priceInfosService.SetPriceInfosBySymbol(tradable.Symbol, new AssetPriceInfos(_random.Next(1,10000)));
            }

            await Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
