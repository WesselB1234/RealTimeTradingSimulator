namespace RealTimeStockSimulator.Models.ViewModels
{
    public class BuyVM
    {
        public Asset Asset { get; set; }
        public int? Amount { get; set; }

        public BuyVM(Asset asset, int? amount)
        {
            Asset = asset;
            Amount = amount;
        }
    }
}
