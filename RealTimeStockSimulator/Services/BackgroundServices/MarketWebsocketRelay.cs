
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Hubs;
using RealTimeStockSimulator.Models;
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
        private IMemoryCache _memoryCache;

        public MarketWebsocketRelay(IConfiguration configuration, IHubContext<MarketHub> hubContext, IMemoryCache memoryCache)
        {
            _marketApiKey = configuration.GetValue<string>("ApiKeyStrings:MarketApiKey");
            _hubContext = hubContext;
            _memoryCache = memoryCache;
        }

        private async Task SubscribeToTradablesInCache(ClientWebSocket client, CancellationToken stoppingToken)
        {
            Dictionary<string, Tradable>? tradablesDictionary = (Dictionary<string, Tradable>?)_memoryCache.Get("TradablesDictionary");

            if (tradablesDictionary == null)
            {
                throw new Exception("Tradables dictionary does not exist.");
            }

            foreach (KeyValuePair<string, Tradable> entry in tradablesDictionary)
            {
                var subscribeRequest = new MarketSubscriptionRequest("subscribe", entry.Value.Symbol);
                string requestJson = JsonSerializer.Serialize(subscribeRequest, _jsonSerializerOptions);

                await client.SendAsync(
                    Encoding.UTF8.GetBytes(requestJson),
                    WebSocketMessageType.Text,
                    true,
                    stoppingToken
                );
            }
        }

        private async Task HandleMarketWebSocketPayload(MarketWebsocketPayload marketPayload, CancellationToken cancellationToken)
        {
            Tradable responseTradable = marketPayload.Data[marketPayload.Data.Count - 1];
            Dictionary<string, Tradable>? tradablesDictionary = (Dictionary<string, Tradable>?)_memoryCache.Get("TradablesDictionary");

            if (tradablesDictionary != null)
            {
                Tradable tradable = tradablesDictionary[responseTradable.Symbol];

                if(tradable.Price != responseTradable.Price)
                {
                    tradable.Price = responseTradable.Price;
                    await _hubContext.Clients.All.SendAsync("ReceiveMarketData", $"{tradable.Symbol}: {tradable.Price}", cancellationToken);
                }
            }
       }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ClientWebSocket client = new ClientWebSocket();
            Uri uri = new Uri($"wss://ws.finnhub.io?token={_marketApiKey}");

            try
            {
                await client.ConnectAsync(uri, cancellationToken);
                await SubscribeToTradablesInCache(client, cancellationToken);

                byte[] buffer = new byte[4096];

                while (!cancellationToken.IsCancellationRequested && client.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    string json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MarketWebsocketPayload? marketPayload = JsonSerializer.Deserialize<MarketWebsocketPayload>(json);
                    
                    if (marketPayload != null && marketPayload.Type == "trade")
                    {
                        await HandleMarketWebSocketPayload(marketPayload, cancellationToken);
                    }    
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when stoppingToken is cancelled — graceful shutdown
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
            finally
            {
                if (client.State == WebSocketState.Open || client.State == WebSocketState.CloseReceived)
                {
                    try
                    {
                        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session completed!", CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error closing WebSocket: {ex.Message}");
                    }
                }

                Console.WriteLine("WebSocket closed. Background service stopping.");
            }
        }
    }
}
