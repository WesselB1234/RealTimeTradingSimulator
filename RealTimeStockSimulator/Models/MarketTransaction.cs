using RealTimeStockSimulator.Models.Enums;
using System.Transactions;

namespace RealTimeStockSimulator.Models
{
    public class MarketTransaction
    {
        public Tradable Tradable { get; set; }
        public decimal Price { get; set; }
        public MarketTransactionStatus Status { get; set; }
        public int Amount {  get; set; }

        public MarketTransaction(Tradable tradable, decimal price, MarketTransactionStatus status, int amount)
        {
            Tradable = tradable;
            Price = price;
            Status = status;
            Amount = amount;
        }
    }
}
