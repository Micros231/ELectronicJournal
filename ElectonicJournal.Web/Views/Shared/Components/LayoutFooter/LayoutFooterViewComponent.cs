using ElectronicJournal.Web.Models.Shared.Components.LayoutFooter;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Views.Shared.Components.LayoutFooter
{
    public class LayoutFooterViewComponent : ViewComponent
    {

        public Task<IViewComponentResult> InvokeAsync()
        {
            var model = new FooterViewModel
            {
                ServerDateTime = DateTime.Now
            };
            
            return Task.FromResult<IViewComponentResult>(View("Default", model));
        }
    }
}
