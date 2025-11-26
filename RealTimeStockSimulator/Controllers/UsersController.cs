using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Services.Interfaces;
using RealTimeStockSimulator.Models;
using Microsoft.AspNetCore.Authorization;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUsersService _usersService;
        private IOwnershipsService _ownershipsService;

        public UsersController(IUsersService usersService, IOwnershipsService ownershipsService)
        {
            _usersService = usersService;
            _ownershipsService = ownershipsService;
        }

        public IActionResult Index()
        {
            return View(_usersService.GetAllUsers());
        }

        public IActionResult Portfolio(int userId)
        {
            UserAccount? user = _usersService.GetUserByUserId(userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User does not exist.";

                return RedirectToAction("Index", "Portfolio");
            }

            Ownership ownership = _ownershipsService.GetOwnershipByUser(user);

            return View(ownership);
        }
    }
}
