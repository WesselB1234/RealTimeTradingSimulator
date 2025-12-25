namespace RealTimeStockSimulator.Repositories
{
    public class DbBaseRepository
    {
        protected readonly string? _connectionString;

        public DbBaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MarketSimulatorDb");
        }
    }
}
