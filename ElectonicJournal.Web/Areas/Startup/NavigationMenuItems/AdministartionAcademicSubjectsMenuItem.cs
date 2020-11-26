using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class AdministartionAcademicSubjectsMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.AdministrationAcademicSubjects,
                ElectronicJournalPageNames.DisplayPageNames.AdministrationAcademicSubjects,
                AreasConsts.Admin + "/AcademicSubjects",
                RolesConsts.Admin.Name);

            return root;
        }
    }
}
