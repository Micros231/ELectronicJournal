using ElectronicJournal.Application.AppService;
using ElectronicJournal.Consts;
using ElectronicJournal.Core.Authorization.Roles;
using ElectronicJournal.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.PrepareInitialize
{
    public class PrepareInitializeAppService : AppServiceBase, IPrepareInitializeAppService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public PrepareInitializeAppService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Initialize()
        {
            var isInit = await IsInitialize();
            if (isInit)
            {
                return;
            }
            await CreateRoles();
            await CreateAdminUser();
            
        }
        public async Task<bool> IsInitialize()
        {
            var isExists = await CheckExsitsRoles();
            if (!isExists)
            {
                return false;
            }
            isExists = await CheckExistsAdminUser();
            if (!isExists)
            {
                return false;
            }
            return true;
        }
        public void Dispose()
        {
            _userManager.Dispose();
            _roleManager.Dispose();
        }
        private async Task CreateAdminUser()
        {
            var isExists = await CheckExistsAdminUser();
            if (isExists)
            {
                return;
            }
            var pass = PrepareInitializeConsts.AdminUser.Password;
            var adminUser = new User { 
                UserName = PrepareInitializeConsts.AdminUser.UserName, 
                Email = PrepareInitializeConsts.AdminUser.Email };
            var result = await _userManager.CreateAsync(adminUser, pass);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, RolesConsts.Admin.Name);
            }
        }
        private async Task CreateRoles()
        {
            await CheckExistsRoleAndCreate(RolesConsts.Admin.Name, RolesConsts.Admin.LocalizedName);
            await CheckExistsRoleAndCreate(RolesConsts.Teacher.Name, RolesConsts.Teacher.LocalizedName);
            await CheckExistsRoleAndCreate(RolesConsts.Student.Name, RolesConsts.Student.LocalizedName);
        }
        private async Task CheckExistsRoleAndCreate(string roleName, string normalizedRoleName)
        {
            var isExists = await CheckExistsRoleByName(roleName);
            if (!isExists)
            {
                await CreateRole(roleName, normalizedRoleName);
            }
        }
        private async Task CreateRole(string roleName, string normalizedRoleName)
        {
            var role = new Role(roleName, normalizedRoleName, string.Empty);
            var result = await _roleManager.CreateAsync(role);

        }
        private async Task<bool> CheckExistsRoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> CheckExsitsRoles()
        {
            var isExists = await CheckExistsRoleByName(RolesConsts.Admin.Name);
            if (!isExists)
            {
                return false;
            }
            isExists = await CheckExistsRoleByName(RolesConsts.Teacher.Name);
            if (!isExists)
            {
                return false;
            }
            isExists = await CheckExistsRoleByName(RolesConsts.Student.Name);
            if (!isExists)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> CheckExistsAdminUser()
        {
            var admins = await _userManager.GetUsersInRoleAsync(RolesConsts.Admin.Name);
            if (admins.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
