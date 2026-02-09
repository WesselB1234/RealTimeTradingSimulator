namespace RealTimeStockSimulator.Models
{
    public class MultiOwnership
    {
        public Dictionary<string, Asset> TradablesDictionary;
        public List<Ownership> Ownerships = new List<Ownership>();

        public MultiOwnership() 
        { 
            TradablesDictionary = new Dictionary<string, Asset>();
            Ownerships = new List<Ownership>();
        }
    }
}
