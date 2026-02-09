using RealTimeStockSimulator.Models.Enums;

namespace RealTimeStockSimulator.Models
{
    public class MarketTransactionTradable
    {
        public int? TransactionId { get; set; }
        public Asset Tradable { get; set; }
        public decimal Price { get; set; }
        public MarketTransactionStatus Status { get; set; }
        public int Amount {  get; set; }
        public DateTime Date { get; set; }

        public MarketTransactionTradable(int transactionId, Asset tradable, decimal price, MarketTransactionStatus status, int amount, DateTime date)
        {
            TransactionId = transactionId;
            Tradable = tradable;
            Price = price;
            Status = status;
            Amount = amount;
            Date = date;
        }

        public MarketTransactionTradable(Asset tradable, decimal price, MarketTransactionStatus status, int amount, DateTime date)
        {
            Tradable = tradable;
            Price = price;
            Status = status;
            Amount = amount;
            Date = date;
        }
    }
}
