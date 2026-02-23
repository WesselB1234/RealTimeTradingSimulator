using Microsoft.AspNetCore.SignalR;
using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;
using System.Text.Json;

namespace RealTimeStockSimulator.Services.BackgroundServiceHandlers
{
    public class MarketWebsocketHandler : IMarketWebsocketHandler
    {
        private IHubContext<MarketHub> _hubContext;
        private IAssetsPriceInfosService _priceInfosService;

        public MarketWebsocketHandler(IHubContext<MarketHub> hubContext, IAssetsPriceInfosService priceInfosService)
        {
            _hubContext = hubContext;
            _priceInfosService = priceInfosService;
        }
        
        public async Task HandleMarketWebSocketPayload(IncomingMarketWebsocketAsset incomingAsset, CancellationToken cancellationToken)
        {
            AssetPriceInfos? assetPriceInfos = _priceInfosService.GetPriceInfosBySymbol(incomingAsset.Symbol);

            if (incomingAsset.Price != null && assetPriceInfos != null && assetPriceInfos.Price != incomingAsset.Price)
            {
                assetPriceInfos.Price = (decimal)incomingAsset.Price;
                AssetUpdatePayload assetUpdatePayload = new AssetUpdatePayload(incomingAsset.Symbol, assetPriceInfos);
                _priceInfosService.SetPriceInfosBySymbol(incomingAsset.Symbol, assetPriceInfos);

                await _hubContext.Clients.All.SendAsync("ReceiveMarketData", JsonSerializer.Serialize(assetUpdatePayload), cancellationToken);
            }
        }
    }
}
