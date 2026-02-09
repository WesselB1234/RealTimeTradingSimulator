using RealTimeStockSimulator.Models.Enums;

namespace RealTimeStockSimulator.Models
{
    public class Asset
    {
        public string Symbol { get; set; }
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
        public AssetType Type { get; set; }
        public AssetPriceInfos? TradablePriceInfos { get; set; }

        public Asset(string symbol, string? name, string? imagePath, AssetType type)
        {
            Symbol = symbol;
            Name = name;
            ImagePath = imagePath;
            Type = type;
        }
    }
}
