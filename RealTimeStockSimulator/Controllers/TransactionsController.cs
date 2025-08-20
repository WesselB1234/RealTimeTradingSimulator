using Microsoft.AspNetCore.Mvc;

namespace RealTimeStockSimulator.Controllers
{
    public class TransactionsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
