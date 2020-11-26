using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class TeacherHomeWorksMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.TeacherHomeWorks,
                ElectronicJournalPageNames.DisplayPageNames.TeacherHomeWorks,
                AreasConsts.Teacher + "/HomeWorks",
                RolesConsts.Teacher.Name);

            return root;
        }
    }
}
