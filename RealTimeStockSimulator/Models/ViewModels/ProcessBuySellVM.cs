namespace RealTimeStockSimulator.Models.ViewModels
{
    public class ProcessBuySellVM
    {
        public string? Symbol { get; set; }
        public int? Amount { get; set; }

        public ProcessBuySellVM(string symbol, int amount)
        {
            Symbol = symbol;
            Amount = amount;
        }

        public ProcessBuySellVM()
        {

        }
    }
}
