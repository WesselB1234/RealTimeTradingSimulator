namespace RealTimeStockSimulator.Models
{
    public class Tradable
    {
        public string Symbol { get; set; }
        public decimal? Price { get; set; }

        public Tradable(string symbol) 
        { 
            Symbol = symbol;
        }
    }
}
