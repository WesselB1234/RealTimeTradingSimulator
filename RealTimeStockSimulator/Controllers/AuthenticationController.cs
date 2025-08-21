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

        public IActionResult Login(LoginViewModel loginViewModel)
        {
            return View(loginViewModel);
        }

        public IActionResult LoginIntoAccount(LoginViewModel loginViewModel)
        {
            User? user = _usersService.GetUserByLoginCredentials(loginViewModel.UserName, loginViewModel.Password);

            if (user != null)
            {
                HttpContext.Session.SetObject("LoggedInUser", user);

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
                User user = new User
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    Money = 0
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

        public IActionResult Logout()
        {
            if (LoggedInUser != null)
            {
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
