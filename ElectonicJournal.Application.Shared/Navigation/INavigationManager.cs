using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public interface INavigationManager
    {
        IDictionary<string, MenuDefinition> Menus { get; }
        MenuDefinition MainMenu { get; }
    }
}
