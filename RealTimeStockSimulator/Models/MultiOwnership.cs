namespace RealTimeStockSimulator.Models
{
    public class MultiOwnership
    {
        public Dictionary<string, Tradable> TradablesDictionary;
        public List<Ownership> Ownerships = new List<Ownership>();

        public MultiOwnership() 
        { 
            TradablesDictionary = new Dictionary<string, Tradable>();
            Ownerships = new List<Ownership>();
        }
    }
}
