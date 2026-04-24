using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Route("Settings")]
    [Authorize]
    public class SettingsController : AuthenticatedUserController
    {
        public SettingsController(IUsersService usersService): base(usersService)
        {
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
