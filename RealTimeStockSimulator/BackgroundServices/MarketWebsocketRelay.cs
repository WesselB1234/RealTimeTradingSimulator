
using Microsoft.AspNetCore.SignalR;
using RealTimeStockSimulator.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace RealTimeStockSimulator.BackgroundServices
{
    public class MarketWebsocketRelay : BackgroundService
    {   
        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ClientWebSocket client = new ClientWebSocket();
            Uri uri = new Uri("wss://ws.finnhub.io?token=");
            await client.ConnectAsync(uri, stoppingToken);

            await client.SendAsync(
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new MarketSubscriptionRequest("subscribe", "BINANCE:BTCUSDT"), _jsonSerializerOptions)),
                WebSocketMessageType.Text,
                true,
                stoppingToken
            );

            byte[] buffer = new byte[4096];

            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine(message);
            }

            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session completed!", stoppingToken);
        }
    }
}
