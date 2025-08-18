using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealTimeStockSimulator.Extensions;
using RealTimeStockSimulator.Models;

namespace RealTimeStockSimulator.Controllers
{
    public class BaseController : Controller
    {
        protected User? LoggedInUser { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LoggedInUser = HttpContext.Session.GetObject<User>("LoggedInUser");

            if (LoggedInUser == null && context.RouteData.Values["controller"]?.ToString() != "Authentication")
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
