using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Controllers
{
    public class TradablesController : BaseController
    {
        private IMemoryCache _memoryCache { get; set; }

        public TradablesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            Dictionary<string, Tradable>? tradablesDictionary = (Dictionary<string, Tradable>?)_memoryCache.Get("TradablesDictionary");

            if (tradablesDictionary == null)
            {
                return NotFound();
            }

            List<Tradable> tradablesList = tradablesDictionary.Values.ToList();

            return View(tradablesList);
        }
    }
}
