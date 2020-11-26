using ElectronicJournal.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicJournal.Application.Navigation
{
    public class MenuItemDefinition : IHasMenuItemDefinitions
    {
        
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public string RoleNameDependency { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public bool IsLeaf => Items.IsNullOrEmpty();
        public string Name { get; }
        public virtual List<MenuItemDefinition> Items { get; }

        public MenuItemDefinition(string name, string displayName, string url, string roleNameDependency)
        {
            Name = name;
            DisplayName = displayName;
            Url = url;
            RoleNameDependency = roleNameDependency;
            IsEnabled = true;
            IsVisible = true;
            RequiresAuthentication = true;
            Items = new List<MenuItemDefinition>();
        }

        public MenuItemDefinition AddItem(MenuItemDefinition menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
        public void RemoveItem(string name)
        {
            Items.RemoveAll(m => m.Name == name);
        }
    }
}
