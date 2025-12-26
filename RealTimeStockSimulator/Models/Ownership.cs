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

        public decimal GetTotalValue(Dictionary<string, Tradable> tradablesDictionary)
        {
            decimal totalValue = 0;

            foreach (KeyValuePair<string, int> kvp in OwnedAmountOfSymbolDictionary)
            {
                string symbol = kvp.Key;
                int amount = kvp.Value;

                if (tradablesDictionary.ContainsKey(symbol))
                {
                    Tradable tradable = tradablesDictionary[symbol];

                    if (tradable.TradablePriceInfos != null)
                    {
                        totalValue += (tradable.TradablePriceInfos.Price * amount);
                    }
                }
            }

            return totalValue;
        }
    }
}
