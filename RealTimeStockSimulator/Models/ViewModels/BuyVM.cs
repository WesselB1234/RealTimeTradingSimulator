namespace RealTimeStockSimulator.Models.ViewModels
{
    public class BuyVM
    {
        public Asset Tradable { get; set; }
        public int? Amount { get; set; }

        public BuyVM(Asset tradable, int? amount)
        {
            Tradable = tradable;
            Amount = amount;
        }
    }
}
