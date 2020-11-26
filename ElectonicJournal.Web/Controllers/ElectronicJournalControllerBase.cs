using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Controllers
{
    public abstract class ElectronicJournalControllerBase : Controller
    {
        protected string NormalizeReturnUrl(string returnUrl, Func<string> defaultValueBuilder = null)
        {
            if (defaultValueBuilder == null)
            {
                defaultValueBuilder = GetDefaultUrl;
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
        protected abstract string GetDefaultUrl();
        protected void AddResultErros(Result result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
        }
        protected void AddIdentityErrors(Result<IdentityResult> result)
        {
            foreach (var identityError in result.Value.Errors)
            {
                ModelState.AddModelError(identityError.Code, identityError.Description);
            }
        }
    }
}