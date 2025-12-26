namespace RealTimeStockSimulator.Models.ViewModels
{
    public class SellVM
    {
        public OwnershipTradable OwnershipTradable { get; set; }
        public int? Amount { get; set; }

        public SellVM(OwnershipTradable ownershipTradable, int? amount)
        {
            OwnershipTradable = ownershipTradable;
            Amount = amount;
        }
    }
}
