using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.Interfaces;
using RealTimeStockSimulator.Repositories.Interfaces;

namespace RealTimeStockSimulator.Repositories
{
    public class DbOwnershipRepository : DbBaseRepository, IOwnershipRepository
    {
        public DbOwnershipRepository(IConfiguration configuration, IDataMapper dataMapper) : base(configuration, dataMapper) { }

        public Ownership GetOwnershipByUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
