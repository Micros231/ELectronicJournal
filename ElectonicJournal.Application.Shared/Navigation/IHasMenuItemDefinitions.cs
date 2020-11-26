using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Navigation
{
    public interface IHasMenuItemDefinitions
    {
        List<MenuItemDefinition> Items { get; }
    }
}
