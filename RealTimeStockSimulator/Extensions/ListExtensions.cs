using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Extensions
{
    public static class ListExtensions
    {
        public static decimal GetTotalOwnershipValue(this List<OwnershipAsset> ownershipTradables)
        {
            decimal total = 0;

            foreach (OwnershipAsset tradable in ownershipTradables)
            {
                total += tradable.TotalValue;
            }

            return total;
        }
    }
}
