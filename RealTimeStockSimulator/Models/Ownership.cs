namespace RealTimeStockSimulator.Models
{
    public class Ownership
    {
        public UserAccount User;
        public Dictionary<string, int> OwnedAmountOfSymbolDictionary;

        public Ownership()
        {
            OwnedAmountOfSymbolDictionary = new Dictionary<string, int>();
        }

        public decimal GetTotalValue(Dictionary<string, Asset> assetsDictionary)
        {
            decimal totalValue = 0;

            foreach (KeyValuePair<string, int> kvp in OwnedAmountOfSymbolDictionary)
            {
                string symbol = kvp.Key;
                int amount = kvp.Value;

                if (assetsDictionary.ContainsKey(symbol))
                {
                    Asset asset = assetsDictionary[symbol];

                    if (asset.AssetPriceInfos != null)
                    {
                        totalValue += (asset.AssetPriceInfos.Price * amount);
                    }
                }
            }

            return totalValue;
        }
    }
}
