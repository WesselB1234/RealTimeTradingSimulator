namespace RealTimeStockSimulator.Models
{
    public class MultiOwnership
    {
        public Dictionary<string, Asset> AssetsDictionary;
        public List<Ownership> Ownerships = new List<Ownership>();

        public MultiOwnership() 
        {
            AssetsDictionary = new Dictionary<string, Asset>();
            Ownerships = new List<Ownership>();
        }
    }
}
