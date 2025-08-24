using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
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

        public IActionResult Buy(ConfirmBuySellViewModel confirmViewModel)
        {
            if (confirmViewModel.Symbol == null)
            {
                return NotFound();
            }

            Tradable? tradable = _tradablesService.GetTradableBySymbol(confirmViewModel.Symbol);

            if (tradable == null)
            {
                return NotFound();
            }

            BuyViewModel viewModel = new BuyViewModel(tradable, confirmViewModel.Amount);

            return View(viewModel);
        }

        public IActionResult ConfirmBuy(ConfirmBuySellViewModel confirmViewModel)
        {
            return RedirectToAction("Buy", confirmViewModel);
        }

        public IActionResult Sell()
        {
            return View();
        }
    }
}
