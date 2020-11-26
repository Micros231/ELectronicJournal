using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Consts;

namespace ElectronicJournal.Web.Areas.Startup.NavigationMenuItems
{
    public class StudentRatingMenuItem : NavigationMenuItemBase
    {
        public override MenuItemDefinition GetMenuItem()
        {
            var root = new MenuItemDefinition(
                ElectronicJournalPageNames.PageNames.StudentRating,
                ElectronicJournalPageNames.DisplayPageNames.StudentRating,
                AreasConsts.Student + "/Rating",
                RolesConsts.Student.Name);

            return root;
        }
    }
}
