namespace RealTimeStockSimulator.Models
{
    public class MultiOwnership
    {
        public Dictionary<string, Tradable> TradablesDictionary = new Dictionary<string, Tradable>();
        public List<ConceptOwnership> Ownerships = new List<ConceptOwnership>();

        public MultiOwnership() { }
    }
}
