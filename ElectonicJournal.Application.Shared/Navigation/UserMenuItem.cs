using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public class UserMenuItem
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public IList<UserMenuItem> Items { get; set; }

        public UserMenuItem()
        {
            
        }
        public UserMenuItem(MenuItemDefinition menuItemDefinition)
        {
            Name = menuItemDefinition.Name;
            DisplayName = menuItemDefinition.DisplayName;
            Url = menuItemDefinition.Url;
            IsEnabled = menuItemDefinition.IsEnabled;
            IsVisible = menuItemDefinition.IsVisible;
            Items = new List<UserMenuItem>();
        }
    }
}
