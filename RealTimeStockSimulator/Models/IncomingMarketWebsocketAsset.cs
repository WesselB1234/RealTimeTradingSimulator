using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class IncomingMarketWebsocketAsset
    {
        [JsonPropertyName("s")]
        public string? Symbol { get; set; }
        [JsonPropertyName("p")]
        public decimal? Price { get; set; }

        public IncomingMarketWebsocketAsset()
        {

        }

        public IncomingMarketWebsocketAsset(string? symbol, decimal? price)
        {
            Symbol = symbol;
            Price = price;
        }
    }
}
