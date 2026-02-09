using RealTimeStockSimulator.Models.Enums;
using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class OwnershipAsset : Asset
    {
        [JsonInclude]
        public int Amount;
        public decimal TotalValue
        {
            get
            {
                if (TradablePriceInfos != null)
                {
                    return TradablePriceInfos.Price * Amount;
                }

                return 0;
            }
        }

        public OwnershipAsset(string symbol, string? name, string? imagePath, AssetType type, int amount) : base(symbol, name, imagePath, type)
        {
            Amount = amount;
        }
    }
}
