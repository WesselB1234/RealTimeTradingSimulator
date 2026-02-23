using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class PortfolioController : AuthenticatedUserController
    {
        private IOwnershipsService _ownershipsService;

        public PortfolioController(IOwnershipsService ownershipsService, IUsersService usersService): base(usersService)
        {
            _ownershipsService = ownershipsService;
        }

        public IActionResult Index()
        {
            return View(_ownershipsService.GetAllOwnershipAssetsByUserId(LoggedInUser.UserId));
        }
    }
}
