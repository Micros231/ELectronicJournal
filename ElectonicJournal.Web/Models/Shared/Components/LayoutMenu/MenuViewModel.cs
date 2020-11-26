using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Models.Shared.Components.LayoutMenu
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }
        public string CurrentPageName { get; set; }
        public UserItemDto User { get; set; }
        public bool IsAuth { get; set; }
    }
}
