using ElectronicJournal.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class CommonInfoMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.CommonInfo,
                ElectronicJournalPageNames.DisplayPageNames.CommonInfo,
                AreasConsts.Common + "/Info",
                null);

            return root;
        }
    }
}
