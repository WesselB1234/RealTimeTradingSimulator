namespace RealTimeStockSimulator.Models
{
    public class AssetUpdatePayload
    {
        public string Symbol { get; set; }
        public AssetPriceInfos TradablePriceInfos { get; set; }

        public AssetUpdatePayload(string symbol, AssetPriceInfos tradablePriceInfos)
        {
            Symbol = symbol;
            TradablePriceInfos = tradablePriceInfos;
        }
    }
}
