using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class MarketWebsocketPayload
    {
        [JsonPropertyName("data")]
        public List<MarketWebsocketTradable> Data { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }

        public MarketWebsocketPayload(List<MarketWebsocketTradable> data, string type)
        {
            Data = data;
            Type = type;
        }
    }
}
