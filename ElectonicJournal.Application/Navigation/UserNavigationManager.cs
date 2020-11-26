using ElectronicJournal.Extenstions;
using ElectronicJournal.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Navigation
{
    public class UserNavigationManager : IUserNavigationManager
    {
        private readonly INavigationManager _navigationManager;
        private readonly UserManager<User> _userManager;
        public UserNavigationManager(
            INavigationManager navigationManager, 
            UserManager<User> userManager)
        {
            _navigationManager = navigationManager;
            _userManager = userManager;
        }
        public async Task<UserMenu> GetMenuAsync(string menuName, ClaimsPrincipal user)
        {
            MenuDefinition menuDefinition = _navigationManager.Menus.GetOrDefault(menuName);
            if (menuDefinition == null)
            {
                throw new Exception("Нет меню с именем:" + menuName);
            }
            var userMenu = new UserMenu(menuDefinition);
            await FillUserMenuItems(user, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }
        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(ClaimsPrincipal user)
        {
            List<UserMenu> userMenus = new List<UserMenu>();
            foreach (MenuDefinition value in _navigationManager.Menus.Values)
            {
                List<UserMenu> list = userMenus;
                list.Add(await GetMenuAsync(value.Name, user));
            }
            return userMenus;
        }
        private async Task<int> FillUserMenuItems(ClaimsPrincipal claimsUser, List<MenuItemDefinition> menuItemDefinitions, IList<UserMenuItem> userMenuItems)
        {
            int addedMenuItemCount = 0;
            var user = await _userManager.GetUserAsync(claimsUser);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var menuItemDefinition in menuItemDefinitions)
            {
                if (!menuItemDefinition.RequiresAuthentication || !(user == null))
                {
                    var containsRole = rolesForUser.Contains(menuItemDefinition.RoleNameDependency);
                    if (string.IsNullOrEmpty(menuItemDefinition.RoleNameDependency))
                    {
                        containsRole = true;
                    }
                    if (containsRole)
                    {
                        var userMenuItem = new UserMenuItem(menuItemDefinition);
                        var isLeaf = menuItemDefinition.IsLeaf;
                        if (!isLeaf)
                        {
                            isLeaf = (await FillUserMenuItems(claimsUser, menuItemDefinition.Items, userMenuItem.Items) > 0);
                        }
                        userMenuItems.Add(userMenuItem);
                        int num = addedMenuItemCount + 1;
                        addedMenuItemCount = num;
                    }
                }
            }
            return addedMenuItemCount;
        }
    }
}
