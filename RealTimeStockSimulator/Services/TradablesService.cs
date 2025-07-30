using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Repositories.Interfaces;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Services
{
    public class TradablesService : ITradablesService
    {
        ITradablesRepository _tradablesRepository;

        public TradablesService(ITradablesRepository tradablesRepository)
        {
            _tradablesRepository = tradablesRepository;
        }

        public List<Tradable> GetAllTradables()
        {
            return _tradablesRepository.GetAllTradables();
        }
    }
}
