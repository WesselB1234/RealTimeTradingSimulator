using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Repositories.Interfaces
{
    public interface IOwnershipRepository
    {
        Ownership GetOwnershipByUser(User user);
    }
}
