using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Controllers
{
    public class AuthenticationController : Controller
    {
        public AuthenticationController()
        {

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

        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            return View(registerViewModel);
        }

        public IActionResult RegisterNewAccount(RegisterViewModel registerViewModel)
        {
            return RedirectToAction("Register", registerViewModel);
        }
    }
}
