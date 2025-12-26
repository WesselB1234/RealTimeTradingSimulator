namespace RealTimeStockSimulator.Models.ViewModels
{
    public class BuyVM
    {
        public Tradable Tradable { get; set; }
        public int? Amount { get; set; }

        public BuyVM(Tradable tradable, int? amount)
        {
            Tradable = tradable;
            Amount = amount;
        }
    }
}
