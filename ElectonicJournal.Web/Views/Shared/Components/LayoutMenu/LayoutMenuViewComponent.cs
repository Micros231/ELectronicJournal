using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Models.Shared.Components.LayoutMenu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Views.Shared.Components.LayoutMenu
{
    public class LayoutMenuViewComponent : ViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IUserAppService _userService;
        private readonly UserManager<User> _userManager;
        public LayoutMenuViewComponent(
            IUserNavigationManager userNavigationManager,
            IUserAppService userService,
            UserManager<User> userManager)
        {
            _userNavigationManager = userNavigationManager;
            _userService = userService;
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(string currentPageName = null)
        {
            bool isAuth = User.Identity.IsAuthenticated;
            var result = await _userService.GetUserByClaims(UserClaimsPrincipal);
            UserItemDto user = new UserItemDto();
            if (result.IsSuccessed)
            {
                user = result.Value;
            }
            var model = new MenuViewModel
            {
                Menu = await _userNavigationManager.GetMenuAsync(ElectronicJournalNavigationProvider.MenuName, UserClaimsPrincipal),
                CurrentPageName = currentPageName,
                IsAuth = isAuth,
                User = user
            };

            return View("Default", model);
        }
    }
}
