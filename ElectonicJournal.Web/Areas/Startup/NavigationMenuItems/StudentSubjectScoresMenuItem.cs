using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class StudentSubjectScoresMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.StudentAcademicSubjectScores,
                ElectronicJournalPageNames.DisplayPageNames.StudentAcademicSubjectScores,
                AreasConsts.Student + "/AcademicSubjectScores",
                RolesConsts.Student.Name);

            return root;
        }
    }
}
