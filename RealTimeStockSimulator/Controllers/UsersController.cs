using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Services.Interfaces;
using RealTimeStockSimulator.Models;
using Microsoft.AspNetCore.Authorization;
using RealTimeStockSimulator.Models.ViewModels;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class UsersController : AuthenticatedUserController
    {
        private IUsersService _usersService;
        private IOwnershipsService _ownershipsService;

        public UsersController(IUsersService usersService, IOwnershipsService ownershipsService): base(usersService)
        {
            _usersService = usersService;
            _ownershipsService = ownershipsService;
        }

        public IActionResult Leaderboard()
        {
            return View(_ownershipsService.GetValueOrderedMultiOwnershipsPagnated(20, 1));
        }

        public IActionResult Portfolio(int userId)
        {
            UserAccount? user = _usersService.GetUserByUserId(userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User does not exist.";

                return RedirectToAction("Index", "Portfolio");
            }

            return View(new UserPortfolioVM(user, _ownershipsService.GetAllOwnershipAssetsByUserId(user.UserId)));
        }
    }
}
