using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class MarketWebsocketTradable
    {
        [JsonPropertyName("s")]
        public string? Symbol { get; set; }
        [JsonPropertyName("p")]
        public decimal? Price { get; set; }

        public MarketWebsocketTradable()
        {

        }
    }
}
