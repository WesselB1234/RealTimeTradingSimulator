using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class TransactionsController : AuthenticatedUserController
    {
        private IMarketTransactionsService _marketTransactionsService;

        public TransactionsController(IMarketTransactionsService marketTransactionsService, IUsersService usersService): base(usersService)
        {
            _marketTransactionsService = marketTransactionsService;
        }

        public IActionResult Index()
        {
            List<MarketTransactionAsset> transactions = _marketTransactionsService.GetTransactionsByUserPagnated(LoggedInUser.UserId, 25, 1);

            return View(transactions);
        }
    }
}
