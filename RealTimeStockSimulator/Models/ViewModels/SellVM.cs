namespace RealTimeStockSimulator.Models.ViewModels
{
    public class SellVM
    {
        public OwnershipAsset OwnershipAsset { get; set; }
        public int? Amount { get; set; }

        public SellVM(OwnershipAsset ownershipAsset, int? amount)
        {
            OwnershipAsset = ownershipAsset;
            Amount = amount;
        }
    }
}
