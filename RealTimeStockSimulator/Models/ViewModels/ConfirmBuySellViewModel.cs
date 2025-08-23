namespace RealTimeStockSimulator.Models.ViewModels
{
    public class ConfirmBuySellViewModel
    {
        public string Symbol { get; set; }
        public int Amount { get; set; }

        public ConfirmBuySellViewModel(string symbol, int amount)
        {
            Symbol = symbol;
            Amount = amount;
        }
    }
}
