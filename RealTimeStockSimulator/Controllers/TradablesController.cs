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

        private Tradable GetTradableFromBuySellViewModel(ConfirmBuySellViewModel confirmViewModel)
        {
            if (confirmViewModel.Symbol == null)
            {
                throw new Exception("Symbol is empty.");
            }

            Tradable? tradable = _tradablesService.GetTradableBySymbol(confirmViewModel.Symbol);

            if (tradable == null)
            {
                throw new Exception("Symbol does not exist.");
            }

            if (tradable.TradablePriceInfos == null)
            {
                throw new Exception("Symbol does not have a price.");
            }

            return tradable;
        }

        private OwnershipTradable GetOwnershipTradableFromBuySellViewModel(ConfirmBuySellViewModel confirmViewModel)
        {
            UserAccount loggedInUser = _usersService.GetUserFromClaimsPrinciple(User);

            if (confirmViewModel.Symbol == null)
            {
                throw new Exception("Symbol is empty.");
            }

            OwnershipTradable? tradable = _ownershipsService.GetOwnershipTradableByUser(loggedInUser, confirmViewModel.Symbol);

            if (tradable == null)
            {
                throw new Exception("Symbol does not exist or you do not own this symbol.");
            }

            if (tradable.TradablePriceInfos == null)
            {
                throw new Exception("Symbol does not have a price.");
            }

            return tradable;
        }

        public IActionResult Buy(ConfirmBuySellViewModel confirmViewModel)
        {
            try
            {
                Tradable tradable = GetTradableFromBuySellViewModel(confirmViewModel);
                BuyViewModel viewModel = new BuyViewModel(tradable, confirmViewModel.Amount);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Portfolio");
            }
        }

        public async Task<IActionResult> ConfirmBuy(ConfirmBuySellViewModel confirmViewModel)
        {
            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                Tradable tradable = GetTradableFromBuySellViewModel(confirmViewModel);
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

                return RedirectToAction("Buy", confirmViewModel);
            }
        }

        public IActionResult Sell(ConfirmBuySellViewModel confirmViewModel)
        {
            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                OwnershipTradable? tradable = GetOwnershipTradableFromBuySellViewModel(confirmViewModel);
                SellViewModel viewModel = new SellViewModel(tradable, confirmViewModel.Amount);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Portfolio");
            }
        }

        public async Task<IActionResult> ConfirmSell(ConfirmBuySellViewModel confirmViewModel)
        {
            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                OwnershipTradable tradable = GetOwnershipTradableFromBuySellViewModel(confirmViewModel);
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

                return RedirectToAction("Sell", confirmViewModel);
            }
        }
    }
}
