using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Extensions
{
    public static class ListExtensions
    {
        public static decimal GetTotalOwnershipValue(this List<OwnershipTradable> ownershipTradables)
        {
            decimal total = 0;

            foreach (OwnershipTradable tradable in ownershipTradables)
            {
                total += tradable.TotalValue;
            }

            return total;
        }
    }
}
