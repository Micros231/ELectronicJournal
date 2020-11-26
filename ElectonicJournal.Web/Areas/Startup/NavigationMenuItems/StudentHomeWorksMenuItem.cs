using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class StudentHomeWorksMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.StudentHomeWorks,
                ElectronicJournalPageNames.DisplayPageNames.StudentHomeWorks,
                AreasConsts.Student + "/HomeWorks",
                RolesConsts.Student.Name);

            return root;
        }
    }
}
