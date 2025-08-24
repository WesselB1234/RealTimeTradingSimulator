namespace RealTimeStockSimulator.Models.ViewModels
{
    public class BuyViewModel
    {
        public Tradable Tradable { get; set; }
        public int? Amount { get; set; }

        public BuyViewModel(Tradable tradable, int? amount)
        {
            Tradable = tradable;
            Amount = amount;
        }
    }
}
