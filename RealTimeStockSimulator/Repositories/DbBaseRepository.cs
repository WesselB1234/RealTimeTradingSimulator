using RealTimeStockSimulator.Models.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbBaseRepository
    {
        protected readonly string? _connectionString;
        protected IDataMapper _dataMapper;

        public DbBaseRepository(IConfiguration configuration, IDataMapper dataMapper)
        {
            _connectionString = configuration.GetConnectionString("MarketSimulatorDb");
            _dataMapper = dataMapper;
        }
    }
}
