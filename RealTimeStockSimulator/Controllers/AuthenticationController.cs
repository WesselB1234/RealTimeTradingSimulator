using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Extensions;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    public class AuthenticationController : BaseController
    {
        private IUsersService _usersService;

        public AuthenticationController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public IActionResult AccessDeniedPath()
        {
            TempData["ErrorMessage"] = "You are not authorized to perform this action.";

            return RedirectToAction("Login");
        }

        public IActionResult Login(LoginViewModel loginViewModel)
        {
            return View(loginViewModel); 
        }

        public async Task<IActionResult> LoginIntoAccount(LoginViewModel loginViewModel)
        {
            UserAccount? user = _usersService.GetUserByLoginCredentials(loginViewModel.UserName, loginViewModel.Password);

            if (user != null)
            {
                HttpContext.Session.SetObject("LoggedInUser", user);

                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(user));

                return RedirectToAction("Index", "Portfolio");
            }

            TempData["ErrorMessage"] = "User does not exist or password is incorrect.";

            return RedirectToAction("Login", loginViewModel);
        }

        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            return View(registerViewModel);
        }

        public IActionResult RegisterNewAccount(RegisterViewModel registerViewModel)
        {
            try
            {
                UserAccount user = new UserAccount
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    Money = 100000
                };

                _usersService.AddUser(user);
                TempData["ConfirmationMessage"] = "Successfully registered a new account.";

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Register", registerViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            if (LoggedInUser != null)
            {
                await HttpContext.SignOutAsync();
                HttpContext.Session.Remove("LoggedInUser");
                TempData["ConfirmationMessage"] = "Successfully logged out.";
            }
            else
            {
                TempData["ErrorMessage"] = "You are not logged in.";
            }

            return RedirectToAction("Login");
        }
    }
}
