namespace RealTimeStockSimulator.Models.ViewModels
{
    public class BuySellViewModel
    {
        public Tradable Tradable { get; set; }
        public int Amount { get; set; }

        public BuySellViewModel(Tradable tradable, int amount)
        {
            Tradable = tradable;
            Amount = amount;
        }
    }
}
