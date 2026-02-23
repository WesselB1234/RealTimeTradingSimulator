using RealTimeStockSimulator.Models.Enums;

namespace RealTimeStockSimulator.Models
{
    public class MarketTransactionAsset
    {
        public int? TransactionId { get; set; }
        public Asset Asset { get; set; }
        public decimal Price { get; set; }
        public MarketTransactionStatus Status { get; set; }
        public int Amount {  get; set; }
        public DateTime Date { get; set; }

        public MarketTransactionAsset(int transactionId, Asset asset, decimal price, MarketTransactionStatus status, int amount, DateTime date)
        {
            TransactionId = transactionId;
            Asset = asset;
            Price = price;
            Status = status;
            Amount = amount;
            Date = date;
        }

        public MarketTransactionAsset(Asset asset, decimal price, MarketTransactionStatus status, int amount, DateTime date)
        {
            Asset = asset;
            Price = price;
            Status = status;
            Amount = amount;
            Date = date;
        }
    }
}
