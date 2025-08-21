namespace RealTimeStockSimulator.Models
{
    public class OwnershipTradable : Tradable
    {
        public int Amount;

        public OwnershipTradable(string symbol, int amount) : base(symbol)
        {
            Amount = amount;
        }
    }
}
