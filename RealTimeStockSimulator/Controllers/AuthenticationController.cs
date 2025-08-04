using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Controllers
{
    public class AuthenticationController : Controller
    {
        private IMemoryCache _memoryCache;

        public AuthenticationController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginIntoAccount()
        {
            Console.WriteLine("login account");

            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterNewAccount()
        {
            Console.WriteLine("register account");

            return RedirectToAction("Register");
        }
    }
}
