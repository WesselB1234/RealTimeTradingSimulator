using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class AssetPriceInfos
    {
        [JsonInclude]
        public decimal Price { get; set; }

        [JsonPropertyName("c")]
        public decimal PriceAlias {
            get
            {
                return Price;
            }
            set
            {
                Price = value;
            } 
        }

        public AssetPriceInfos(decimal price)
        {
            Price = price;
        }

        public AssetPriceInfos()
        {

        }
    }
}
