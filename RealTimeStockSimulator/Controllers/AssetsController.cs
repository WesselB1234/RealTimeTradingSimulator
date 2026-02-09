using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    public class AssetsController : AuthenticatedUserController
    {
        private IAssetsService _tradablesService;
        private IOwnershipsService _ownershipsService;
        private IUsersService _usersService;

        public AssetsController(IAssetsService tradablesService, IOwnershipsService ownershipsService, IUsersService usersService): base(usersService)
        {
            _tradablesService = tradablesService;
            _ownershipsService = ownershipsService;
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            List<Asset> tradables = _tradablesService.GetAllTradables();

            return View(tradables);
        }

        public IActionResult Buy(ProcessBuySellVM confirmViewModel)
        {
            try
            {
                Asset tradable = _tradablesService.GetTradableFromBuySellViewModel(confirmViewModel);
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
            Asset? tradable = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                tradable = _tradablesService.GetTradableFromBuySellViewModel(confirmViewModel);
                decimal moneyAfterPurchase = _ownershipsService.BuyTradable(LoggedInUser, tradable, (int)confirmViewModel.Amount);

                LoggedInUser.Money = moneyAfterPurchase;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(LoggedInUser));
                _usersService.UpdateBalanceByUserId(LoggedInUser.UserId, moneyAfterPurchase);

                TempData["ConfirmationMessage"] = $"Successful bought {(confirmViewModel.Amount > 1 ? $"{confirmViewModel.Amount}x copies" : "1x copy")} of {confirmViewModel.Symbol}.";

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

                OwnershipAsset? tradable = _ownershipsService.GetOwnershipTradableFromBuySellViewModel(confirmViewModel, LoggedInUser.UserId);
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
            OwnershipAsset? tradable = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                tradable = _ownershipsService.GetOwnershipTradableFromBuySellViewModel(confirmViewModel,LoggedInUser.UserId);
                decimal moneyAfterSelling = _ownershipsService.SellTradable(LoggedInUser, tradable, (int)confirmViewModel.Amount);

                LoggedInUser.Money = moneyAfterSelling;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(LoggedInUser));
                _usersService.UpdateBalanceByUserId(LoggedInUser.UserId, moneyAfterSelling);

                TempData["ConfirmationMessage"] = $"Successful sold {(confirmViewModel.Amount > 1 ? $"{confirmViewModel.Amount}x copies" : "1x copy")} of {confirmViewModel.Symbol}.";

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
