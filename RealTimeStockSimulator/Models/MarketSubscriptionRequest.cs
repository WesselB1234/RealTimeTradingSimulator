namespace RealTimeStockSimulator.Models
{
    public class MarketSubscriptionRequest
    {
        public string Type { get; set; }
        public string Symbol { get; set; }

        public MarketSubscriptionRequest(string type, string symbol)
        {
            Type = type;
            Symbol = symbol;
        }
    }
}
