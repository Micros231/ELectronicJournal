using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public class MenuDefinition : IHasMenuItemDefinitions
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<MenuItemDefinition> Items { get; }

        public MenuDefinition(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
            Items = new List<MenuItemDefinition>();
        }

        public MenuDefinition AddItem(MenuItemDefinition menuItem)
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
