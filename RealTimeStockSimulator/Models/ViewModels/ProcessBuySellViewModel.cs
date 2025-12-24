namespace RealTimeStockSimulator.Models.ViewModels
{
    public class ProcessBuySellViewModel
    {
        public string? Symbol { get; set; }
        public int? Amount { get; set; }

        public ProcessBuySellViewModel(string symbol, int amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        public ProcessBuySellViewModel()
        {

        }
    }
}
