using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login(string returnUrl = "")
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(new LoginViewModel());
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel loginModel, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                returnUrl = NormalizeReturnUrl(returnUrl);
                var result =
                    await _signInManager.PasswordSignInAsync(
                        loginModel.UserName,
                        loginModel.UserPassword,
                        loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(loginModel);
        }
        public async Task<IActionResult> Logout(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            return Redirect(returnUrl);
        }
        private string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetAppHomeUrl;
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return defaultValueBuilder();
            }
            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }
            return defaultValueBuilder();
        }
        private string GetAppHomeUrl()
        {
            return Url.Action("Index", "Welcome", new { Area = "Common" });
        }

    }
}