using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public abstract class NavigationProvider 
    {
        protected NavigationProvider() { }
        public abstract void SetNavigation(INavigationProviderContext context);
    }
}
