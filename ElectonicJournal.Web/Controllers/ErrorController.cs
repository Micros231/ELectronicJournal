using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Web.Models.Error;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int? code = null)
        {
            if (code.HasValue)
            {
                HttpContext.Response.StatusCode = code.Value;
            }
            var model = new ErrorViewModel
            {
                StatusCode = code
            };
            return View(model);
        }
    }
}
