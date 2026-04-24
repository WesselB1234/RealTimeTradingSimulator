using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    [Authorize]
    [Route("Assets")]
    public class AssetsController : AuthenticatedUserController
    {
        private IAssetsService _assetsService;
        private IOwnershipsService _ownershipsService;
        private IUsersService _usersService;

        public AssetsController(IAssetsService assetsService, IOwnershipsService ownershipsService, IUsersService usersService): base(usersService)
        {
            _assetsService = assetsService;
            _ownershipsService = ownershipsService;
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Asset> assets = _assetsService.GetAllAssets();

            return View(assets);
        }

        [HttpGet("Buy/{symbol}")]
        public IActionResult Buy(string symbol)
        {
            try
            {
                Asset asset = _assetsService.GetAssetBySymbolOrThrow(symbol);
                BuyVM viewModel = new BuyVM(asset, 1);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Portfolio");
            }
        }

        [HttpPost("ProcessBuy/{symbol}")]
        public async Task<IActionResult> ProcessBuy(string symbol, ProcessBuySellVM confirmViewModel)
        {   
            Asset? asset = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                asset = _assetsService.GetAssetBySymbolOrThrow(symbol);
                decimal moneyAfterPurchase = _ownershipsService.BuyAsset(LoggedInUser, asset, (int)confirmViewModel.Amount);

                LoggedInUser.Money = moneyAfterPurchase;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(LoggedInUser));
                _usersService.UpdateBalanceByUserId(LoggedInUser.UserId, moneyAfterPurchase);

                TempData["ConfirmationMessage"] = $"Successful bought {(confirmViewModel.Amount > 1 ? $"{confirmViewModel.Amount}x copies" : "1x copy")} of {confirmViewModel.Symbol}.";

                return RedirectToAction("Index", "Portfolio");
            }
            catch (Exception ex)
            {
                confirmViewModel.Symbol = symbol; 

                TempData["ErrorMessage"] = ex.Message;

                if (asset != null)
                {
                    BuyVM viewModel = new BuyVM(asset, confirmViewModel.Amount);
                    return View("Buy", viewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Portfolio");
                }
            }
        }

        [HttpGet("Sell/{symbol}")]
        public string Sell(string symbol)//(ProcessBuySellVM confirmViewModel)
        {
            return symbol;

            //try
            //{
            //    if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
            //    {
            //        confirmViewModel.Amount = 1;
            //    }

            //    OwnershipAsset? ownershipAsset = _ownershipsService.GetOwnershipAssetFromBuySellViewModel(confirmViewModel, LoggedInUser.UserId);
            //    SellVM viewModel = new SellVM(ownershipAsset, confirmViewModel.Amount);

            //    return View(viewModel);
            //}
            //catch (Exception ex)
            //{
            //    TempData["ErrorMessage"] = ex.Message;

            //    return RedirectToAction("Index", "Portfolio");
            //}
        }

        [HttpPost]
        public async Task<IActionResult> ProcessSell(ProcessBuySellVM confirmViewModel)
        {
            OwnershipAsset? ownershipAsset = null;

            try
            {
                if (confirmViewModel.Amount == null || confirmViewModel.Amount < 1)
                {
                    confirmViewModel.Amount = 1;
                }

                ownershipAsset = _ownershipsService.GetOwnershipAssetFromBuySellViewModel(confirmViewModel, LoggedInUser.UserId);
                decimal moneyAfterSelling = _ownershipsService.SellAsset(LoggedInUser, ownershipAsset, (int)confirmViewModel.Amount);

                LoggedInUser.Money = moneyAfterSelling;
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(LoggedInUser));
                _usersService.UpdateBalanceByUserId(LoggedInUser.UserId, moneyAfterSelling);

                TempData["ConfirmationMessage"] = $"Successful sold {(confirmViewModel.Amount > 1 ? $"{confirmViewModel.Amount}x copies" : "1x copy")} of {confirmViewModel.Symbol}.";

                return RedirectToAction("Index", "Portfolio");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                if (ownershipAsset != null)
                {
                    SellVM viewModel = new SellVM(ownershipAsset, confirmViewModel.Amount);
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
