using ElectronicJournal.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Models.Shared.Components.LayoutMenu
{
    public class MenuItemViewModel
    {
        public UserMenuItem MenuItem { get; set; }
        public string CurrentPageName { get; set; }
        public int MenuItemIndex { get; set; }
    }
}
