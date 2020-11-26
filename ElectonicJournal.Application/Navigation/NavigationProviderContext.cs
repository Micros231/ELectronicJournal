using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public class NavigationProviderContext : INavigationProviderContext
    {
        public INavigationManager Manager { get; private set; }

        public NavigationProviderContext(INavigationManager manager)
        {
            Manager = manager;
        }
    }
}
