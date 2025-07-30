using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class Tradable
    {
        public string Symbol { get; set; }
        [JsonPropertyName("c")]
        public decimal? Price { get; set; }

        public Tradable(string symbol) 
        { 
            Symbol = symbol;
        }
    }
}
