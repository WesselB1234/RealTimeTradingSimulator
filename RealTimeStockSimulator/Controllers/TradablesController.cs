using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class TradablesController : Controller
    {
        private ITradablesService _tradablesService;
        private IOwnershipsService _ownershipsService;
        private IUsersService _usersService;
        private UserAccount _loggedInUser;

        public TradablesController(ITradablesService tradablesService, IOwnershipsService ownershipsService, IUsersService usersService)
        {
            _tradablesService = tradablesService;
            _ownershipsService = ownershipsService;
            _usersService = usersService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _loggedInUser = _usersService.GetUserFromClaimsPrinciple(User);

            ViewBag.loggedInUser = _loggedInUser;
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            List<Tradable> tradables = _tradablesService.GetAllTradables();

            return View(tradables);
        }

        public IActionResult Buy(ProcessBuySellVM confirmViewModel)
        {
            try
            {
                Tradable tradable = _tradablesService.GetTradableFromBuySellViewModel(confirmViewModel);
                BuyVM viewModel = new BuyVM(tradable, confirmViewModel.Amount);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Portfolio");
            }
        }

        public async Task<IActionResult> ProcessBuy(ProcessBuySellVM confirmViewModel)
        {
            Tradable? tradable = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                tradable = _tradablesService.GetTradableFromBuySellViewModel(confirmViewModel);
                decimal moneyAfterPurchase = _ownershipsService.BuyTradable(_loggedInUser, tradable, (int)confirmViewModel.Amount);

                _loggedInUser.Money = moneyAfterPurchase;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(_loggedInUser));
                _usersService.UpdateBalanceByUserId(_loggedInUser.UserId, moneyAfterPurchase);

                TempData["ConfirmationMessage"] = "Buying successful.";

                return RedirectToAction("Index", "Portfolio");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                if (tradable != null)
                {
                    BuyVM viewModel = new BuyVM(tradable, confirmViewModel.Amount);
                    return View("Buy", viewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Portfolio");
                }
            }
        }

        public IActionResult Sell(ProcessBuySellVM confirmViewModel)
        {
            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                OwnershipTradable? tradable = _ownershipsService.GetOwnershipTradableFromBuySellViewModel(confirmViewModel, _loggedInUser.UserId);
                SellVM viewModel = new SellVM(tradable, confirmViewModel.Amount);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Portfolio");
            }
        }

        public async Task<IActionResult> ProcessSell(ProcessBuySellVM confirmViewModel)
        {
            OwnershipTradable? tradable = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                tradable = _ownershipsService.GetOwnershipTradableFromBuySellViewModel(confirmViewModel,_loggedInUser.UserId);
                decimal moneyAfterSelling = _ownershipsService.SellTradable(_loggedInUser, tradable, (int)confirmViewModel.Amount);

                _loggedInUser.Money = moneyAfterSelling;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(_loggedInUser));
                _usersService.UpdateBalanceByUserId(_loggedInUser.UserId, moneyAfterSelling);

                TempData["ConfirmationMessage"] = "Selling successful.";

                return RedirectToAction("Index", "Portfolio");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                if (tradable != null)
                {
                    SellVM viewModel = new SellVM(tradable, confirmViewModel.Amount);
                    return View("Sell", viewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Portfolio");
                }
            }
        }
    }
}
