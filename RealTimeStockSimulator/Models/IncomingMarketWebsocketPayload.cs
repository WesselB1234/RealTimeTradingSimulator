using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class IncomingMarketWebsocketPayload
    {
        [JsonPropertyName("data")]
        public List<IncomingMarketWebsocketAsset> Data { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }

        public IncomingMarketWebsocketPayload(List<IncomingMarketWebsocketAsset> data, string type)
        {
            Data = data;
            Type = type;
        }
    }
}
