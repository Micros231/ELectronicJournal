using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class TeacherSubjectScoresMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.TeacherAcademicSubjectScores,
                ElectronicJournalPageNames.DisplayPageNames.TeacherAcademicSubjectScores,
                AreasConsts.Teacher + "/AcademicSubjectScores",
                RolesConsts.Teacher.Name);

            return root;
        }
    }
}
