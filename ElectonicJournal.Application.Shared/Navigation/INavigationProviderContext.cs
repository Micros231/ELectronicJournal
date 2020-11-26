using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public interface INavigationProviderContext
    {
        INavigationManager Manager { get; }
    }
}
