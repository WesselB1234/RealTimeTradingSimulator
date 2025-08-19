using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class TradablePriceInfos
    {
        [JsonInclude]
        public decimal Price { get; set; }

        [JsonPropertyName("c")]
        public decimal Price2 {
            get
            {
                return Price;
            }
            set
            {
                Price = value;
            } 
        }

        public TradablePriceInfos(decimal price)
        {
            Price = price;
        }

        public TradablePriceInfos()
        {

        }
    }
}
