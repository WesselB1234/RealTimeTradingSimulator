using System.Globalization;

namespace RealTimeStockSimulator.Models.Helpers
{
    public static class StringFormatter
    {
        public static string FormatDecimalPrice(decimal price)
        {
            return price.ToString("#,##0.00", new CultureInfo("en-US"));
        }

        public static string FormatDecimalToJsDecimal(decimal price)
        {
            return price.ToString(CultureInfo.InvariantCulture);
        }
    }
}
