using Microsoft.Data.SqlClient;

namespace RealTimeStockSimulator.Models.Interfaces
{
    public interface IDataMapper
    {
        User MapUser(SqlDataReader reader);
        Tradable MapTradable(SqlDataReader reader);
    }
}
