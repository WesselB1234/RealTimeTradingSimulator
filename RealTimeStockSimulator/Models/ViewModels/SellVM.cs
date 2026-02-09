namespace RealTimeStockSimulator.Models.ViewModels
{
    public class SellVM
    {
        public OwnershipAsset OwnershipTradable { get; set; }
        public int? Amount { get; set; }

        public SellVM(OwnershipAsset ownershipTradable, int? amount)
        {
            OwnershipTradable = ownershipTradable;
            Amount = amount;
        }
    }
}
