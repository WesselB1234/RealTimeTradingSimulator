using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private IMarketTransactionsService _marketTransactionsService;

        public TransactionsController(IMarketTransactionsService marketTransactionsService)
        {
            _marketTransactionsService = marketTransactionsService;
        }

        public IActionResult Index()
        {
            MarketTransactions transactions = _marketTransactionsService.GetTransactionsByUserPagnated(LoggedInUser);

            return View(transactions);
        }
    }
}
