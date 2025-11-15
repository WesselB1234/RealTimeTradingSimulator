
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories.Interfaces;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace RealTimeStockSimulator.Services.BackgroundServices
{
    public class MarketWebsocketRelay : BackgroundService
    {   
        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private string? _marketApiKey;
        private IHubContext<MarketHub> _hubContext;
        private ITradablePriceInfosService _priceInfosService;

        public MarketWebsocketRelay(IConfiguration configuration, IHubContext<MarketHub> hubContext, ITradablePriceInfosService priceInfosService)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _hubContext = hubContext;
            _priceInfosService = priceInfosService;
        }

        private async Task SubscribeToTradablesInCache(ClientWebSocket client)
        {
            foreach (string key in _priceInfosService.GetAllKeys())
            {
                MarketSubscriptionRequest subscribeRequest = new MarketSubscriptionRequest("subscribe", key);
                string requestJson = JsonSerializer.Serialize(subscribeRequest, _jsonSerializerOptions);

                await client.SendAsync(Encoding.UTF8.GetBytes(requestJson), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task HandleMarketWebSocketPayload(MarketWebsocketPayload marketPayload)
        {
            MarketWebsocketTradable responseTradable = marketPayload.Data[marketPayload.Data.Count - 1];
            TradablePriceInfos? tradablePriceInfos = _priceInfosService.GetPriceInfosBySymbol(responseTradable.Symbol);

            if (responseTradable.Price != null && tradablePriceInfos != null && tradablePriceInfos.Price != responseTradable.Price)
            {
                tradablePriceInfos.Price = (decimal)responseTradable.Price;
                TradableUpdatePayload tradableUpdatePayload = new TradableUpdatePayload(responseTradable.Symbol, tradablePriceInfos);

                _priceInfosService.SetPriceInfosBySymbol(responseTradable.Symbol, tradablePriceInfos);

                await _hubContext.Clients.All.SendAsync("ReceiveMarketData", JsonSerializer.Serialize(tradableUpdatePayload), CancellationToken.None);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ClientWebSocket client = new ClientWebSocket();

            try
            {
                Uri uri = new Uri($"wss://ws.finnhub.io?token={_marketApiKey}");

                await client.ConnectAsync(uri, CancellationToken.None);
                await SubscribeToTradablesInCache(client);

                byte[] buffer = new byte[4096];
 
                while (!cancellationToken.IsCancellationRequested && client.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MarketWebsocketPayload? marketPayload = JsonSerializer.Deserialize<MarketWebsocketPayload>(json);

                    if (marketPayload != null && marketPayload.Type == "trade")
                    {
                        await HandleMarketWebSocketPayload(marketPayload);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Market websocket error: {ex.Message}");
            }

            try
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session completed!", CancellationToken.None);
                    Console.WriteLine("Successfully closed the market websocket.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Market websocket closing error: {ex.Message}");
            }

            client.Dispose();
        }
    }
}
