using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Web.Areas.Startup.NavigationMenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Startup
{
    public class ElectronicJournalNavigationProvider : NavigationProvider
    {
        public const string MenuName = "ElectronicJournal";
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, "Электронный журнал");

            foreach (var menuItem in GetNavigationMenuItems())
            {
                menu.AddItem(menuItem.GetMenuItem());
            }
        }

        private IReadOnlyList<NavigationMenuItemBase> GetNavigationMenuItems()
        {
            var menuItems = new List<NavigationMenuItemBase>();
            //Admins
            menuItems.Add(new AdministationUsersMenuItem());
            menuItems.Add(new AdministrationStudyGroupsMenuItem());
            menuItems.Add(new AdministartionAcademicSubjectsMenuItem());

            //Teachers
            menuItems.Add(new TeacherSubjectScoresMenuItem());
            menuItems.Add(new TeacherHomeWorksMenuItem());

            //Students
            menuItems.Add(new StudentSubjectScoresMenuItem());
            menuItems.Add(new StudentHomeWorksMenuItem());
            menuItems.Add(new StudentRatingMenuItem());

            //Shared
            menuItems.Add(new CommonInfoMenuItem());
            return menuItems;
        }
    }
}
