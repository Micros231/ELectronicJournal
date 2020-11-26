using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public class NavigationManager : INavigationManager
    {
        public IDictionary<string, MenuDefinition> Menus { get; private set; }

        public MenuDefinition MainMenu => Menus["MainMenu"];

        public NavigationManager()
        {
            Menus = new Dictionary<string, MenuDefinition>
            {
                {
                    "MainMenu",
                    new MenuDefinition("MainMenu", "Главное меню")
                }
            };
        }
    }
}
