using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    public class TradablesController : BaseController
    {
        private ITradablesService _tradablesService { get; set; }

        public TradablesController(ITradablesService tradablesService)
        {
            _tradablesService = tradablesService;
        }

        public IActionResult Index()
        {
            List<Tradable> tradables = _tradablesService.GetAllTradables();

            return View(tradables);
        }

        public IActionResult Buy(string? symbol)
        {
            if (symbol == null)
            {
                return NotFound();
            }

            Tradable? tradable = _tradablesService.GetTradableBySymbol(symbol);

            if (tradable == null)
            {
                return NotFound();
            }

            return View(tradable);
        }

        public IActionResult Sell()
        {
            return View();
        }
    }
}
