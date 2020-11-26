using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class AdministationUsersMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.AdministrationUsers,
                ElectronicJournalPageNames.DisplayPageNames.AdministrationUsers,
                AreasConsts.Admin + "/Users",
                RolesConsts.Admin.Name);
            return root;
        }
    }
}
