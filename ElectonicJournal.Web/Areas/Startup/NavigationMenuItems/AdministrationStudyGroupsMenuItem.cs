using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class AdministrationStudyGroupsMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.AdministrationStudyGroups,
                ElectronicJournalPageNames.DisplayPageNames.AdministrationStudyGroups,
                AreasConsts.Admin + "/StudyGroups",
                RolesConsts.Admin.Name);

            return root;
        }
    }
}
