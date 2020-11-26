using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElectronicJournal.Web.Models;
using ElectronicJournal.Application.PrepareInitialize;
using Microsoft.AspNetCore.Identity;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Extenstions;

namespace ElectronicJournal.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var redirectoToCommonArea = RedirectToAction("Index", "Welcome", new { Area = "Common" });
            var redirectToLogin = RedirectToAction("Login", "Account");
            return User.Identity.IsAuthenticated ? 
               redirectoToCommonArea : redirectToLogin ;

        }
    }
}
