using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Web.Areas.Admin.Models.Users;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Extenstions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ElectronicJournal.Web.Controllers;

namespace ElectronicJournal.Web.Areas.Admin.Controllers
{
    [Area(AreasConsts.Admin)]
    [Authorize(Roles = RolesConsts.Admin.Name)]
    public class UsersController : ElectronicJournalControllerBase
    {
        private readonly IUserAppService _userService;
        public UsersController(
            IUserAppService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetUsersViewModel();
            foreach (var item in model.Input.RoleComboboxItems.Items)
            {
                item.IsSelected = true;
            }
            var result = await _userService.GetUsers(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetUsersViewModel model)
        {
            var result = await _userService.GetUsers(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        public IActionResult CreateUser(string returnUrl = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var model = new CreateUserViewModel
            {
                Input = new CreateUserInput(),
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUser(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
                AddIdentityErrors(result);
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfo(long id)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _userService.GetUserById(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var user = result.Value;
                var model = new EditInfoUserViewModel();
                model.Input.UserId = user.Id;
                model.Input.UserName = user.UserName;
                model.Input.Email = user.Email;
                model.Input.FirstName = user.FirstName;
                model.Input.LastName = user.LastName;
                model.ReturnUrl = returnUrl;
                foreach (var role in user.Roles.Items)
                {
                    var roleCombobox =
                        model.Input.RoleComboboxItems.Items.FirstOrDefault(
                            roleCombobox => roleCombobox.Value == role.Name);
                    if (roleCombobox != null)
                    {
                        roleCombobox.IsSelected = true;
                    }
                }
                return View(model);
            }
            return Redirect(GetDefaultUrl());
        }
        [HttpPost]
        public async Task<IActionResult> EditInfo(EditInfoUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
                AddIdentityErrors(result);
            }
            return View(model);

        }
        public async Task<IActionResult> EditPassword(long id)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _userService.GetUserById(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var user = result.Value;
                var model = new EditPasswordUserViewModel();
                model.Input.UserId = user.Id;
                model.UserName = user.UserName;
                model.ReturnUrl = returnUrl;
                return View(model);
            }
            return Redirect(GetDefaultUrl());
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserPassword(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
                AddIdentityErrors(result);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var result = await _userService.DeleteUser(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            throw new Exception(result.Errors.First().Message);
        }
        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "Users", new { Area = AreasConsts.Admin });
        }
    }
}