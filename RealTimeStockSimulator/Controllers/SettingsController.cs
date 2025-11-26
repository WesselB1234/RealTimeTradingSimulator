using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
