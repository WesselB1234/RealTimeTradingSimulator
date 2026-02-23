namespace RealTimeStockSimulator.Models
{
    public class AssetUpdatePayload
    {
        public string Symbol { get; set; }
        public AssetPriceInfos AssetPriceInfos { get; set; }

        public AssetUpdatePayload(string symbol, AssetPriceInfos assetPriceInfos)
        {
            Symbol = symbol;
            AssetPriceInfos = assetPriceInfos;
        }
    }
}
