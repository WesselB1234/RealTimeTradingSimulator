namespace RealTimeStockSimulator.Models.ViewModels
{
    public class SellViewModel
    {
        public OwnershipTradable OwnershipTradable { get; set; }
        public int? Amount { get; set; }

        public SellViewModel(OwnershipTradable ownershipTradable, int amount)
        {
            OwnershipTradable = ownershipTradable;
            Amount = amount;
        }
    }
}
