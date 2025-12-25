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
        private ITradablePriceInfosService _priceInfosService;

        public MarketWebsocketHandler(IHubContext<MarketHub> hubContext, ITradablePriceInfosService priceInfosService)
        {
            _hubContext = hubContext;
            _priceInfosService = priceInfosService;
        }

        public async Task HandleMarketWebSocketPayload(IncomingMarketWebsocketTradable incomingTradable)
        {
            TradablePriceInfos? tradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(incomingTradable.Symbol);

            if (incomingTradable.Price != null && tradablePriceInfos != null && tradablePriceInfos.Price != incomingTradable.Price)
            {
                tradablePriceInfos.Price = (decimal)incomingTradable.Price;
                TradableUpdatePayload tradableUpdatePayload = new TradableUpdatePayload(incomingTradable.Symbol, tradablePriceInfos);
                _priceInfosService.SetPriceInfosBySymbol(incomingTradable.Symbol, tradablePriceInfos);

                await _hubContext.Clients.All.SendAsync("ReceiveMarketData", JsonSerializer.Serialize(tradableUpdatePayload), CancellationToken.None);
            }
        }
    }
}
