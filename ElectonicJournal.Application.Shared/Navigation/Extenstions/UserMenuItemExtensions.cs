using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation.Extenstions
{
    public static class UserMenuItemExtensions
    {
        public static bool IsMenuActive(this UserMenuItem menuItem, string pageName)
        {
            return menuItem.Name.Equals(pageName);
        }
        public static string CalculateUrl(this UserMenuItem menuItem, string rootUrl)
        {
            return rootUrl + menuItem.Url;
        }
    }
}
