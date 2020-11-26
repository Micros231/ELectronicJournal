using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Web.Areas.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Areas.Common.Controllers
{
    [Area(AreasConsts.Common)]
    [Authorize]
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}