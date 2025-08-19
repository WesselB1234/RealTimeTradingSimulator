using System.Text.Json.Serialization;

namespace RealTimeStockSimulator.Models
{
    public class Tradable
    {
        public string Symbol { get; set; }
        public TradablePriceInfos? TradablePriceInfos { get; set; }

        public Tradable(string symbol) 
        { 
            Symbol = symbol;
        }
    }
}
