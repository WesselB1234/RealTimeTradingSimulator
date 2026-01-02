using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeStockSimulator.Models;
using RealTimeStockSimulator.Models.ViewModels;
using RealTimeStockSimulator.Services.Interfaces;

namespace RealTimeStockSimulator.Controllers
{
    public class AuthenticationController : Controller
    {
        private IUsersService _usersService;

        public AuthenticationController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public IActionResult NotAuthorized()
        {
            TempData["ErrorMessage"] = "Your account is not authorized to perform this action.";

            return RedirectToAction("Login");
        }

        public IActionResult NotLoggedIn()
        {
            TempData["ErrorMessage"] = "You must be logged in to perform this action.";

            return RedirectToAction("Login");
        }

        public IActionResult Login(LoginVM loginViewModel)
        {
            return View(loginViewModel); 
        }

        public async Task<IActionResult> ProcessLogin(LoginVM loginViewModel)
        {
            UserAccount? user = _usersService.GetUserByLoginCredentials(loginViewModel.UserName, loginViewModel.Password);

            if (user != null)
            {
                await HttpContext.SignInAsync(_usersService.GetClaimsPrincipleFromUser(user));
                return RedirectToAction("Index", "Portfolio");
            }

            TempData["ErrorMessage"] = "User does not exist or password is incorrect.";

            return View("Login", loginViewModel);
        }

        public IActionResult Register(RegisterVM registerViewModel)
        {
            return View(registerViewModel);
        }

        public async Task<IActionResult> ProcessRegister(RegisterVM registerViewModel)
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

                return await ProcessLogin(new LoginVM(registerViewModel.UserName, registerViewModel.Password));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message; 
                
                return View("register", registerViewModel);
            }
        }

        [Authorize]        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            TempData["ConfirmationMessage"] = "Successfully logged out.";

            return View("Login");
        }
    }
}
