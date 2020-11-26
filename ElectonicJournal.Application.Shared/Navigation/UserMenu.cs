using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public class UserMenu
    {
        

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IList<UserMenuItem> Items { get; set; }

        public UserMenu()
        {
            
        }
        public UserMenu(MenuDefinition menuDefinition)
        {
            Name = menuDefinition.Name;
            DisplayName = menuDefinition.DisplayName;
            Items = new List<UserMenuItem>();
        }
    }
}
