using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private IOwnershipsService _ownershipsService;
        private IUsersService _usersService;

        public PortfolioController(IOwnershipsService ownershipsService, IUsersService usersService)
        {
            _ownershipsService = ownershipsService;
            _usersService = usersService;
        }
        
        public IActionResult Index()
        {
            UserAccount loggedInUser = _usersService.GetUserFromClaimsPrinciple(User);

            return View(_ownershipsService.GetOwnershipByUser(loggedInUser));
        }
    }
}
