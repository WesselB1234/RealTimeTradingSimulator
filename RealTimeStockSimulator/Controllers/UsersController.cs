using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Services.Interfaces;
using RealTimeStockSimulator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUsersService _usersService;
        private IOwnershipsService _ownershipsService;
        private UserAccount _loggedInUser;

        public UsersController(IUsersService usersService, IOwnershipsService ownershipsService)
        {
            _usersService = usersService;
            _ownershipsService = ownershipsService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _loggedInUser = _usersService.GetUserFromClaimsPrinciple(User);

            ViewBag.loggedInUser = _loggedInUser;
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            return View(_ownershipsService.GetOrderedOwnershipsPagnated(20, 1));
        }

        public IActionResult Portfolio(int userId)
        {
            UserAccount? user = _usersService.GetUserByUserId(userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User does not exist.";

                return RedirectToAction("Index", "Portfolio");
            }

            Ownership ownership = new Ownership(user, _ownershipsService.GetAllOwnershipTradablesByUserId(user.UserId));

            return View(ownership);
        }
    }
}
