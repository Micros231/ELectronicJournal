using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Core.Authorization.Roles;
using ElectronicJournal.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Extenstions;
using System.Security.Claims;

namespace ElectronicJournal.Application.Authorization.Users
{
    public class UserAppService : AppServiceBase<User, UserItemDto, long>, IUserAppService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IStudentAppService _studentService;
        private readonly ITeacherAppService _teacherService;
        public UserAppService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IStudentAppService studentAppService,
            ITeacherAppService teacherAppService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _studentService = studentAppService;
            _teacherService = teacherAppService;
        }

        public async Task<Result<IdentityResult>> CreateUser(CreateUserInput input)
        {
            var createdUser = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName
            };
            var result = await _userManager.CreateAsync(createdUser, input.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(createdUser.Email);
                foreach (var roleCombobox in input.RoleComboboxItems.Items)
                {
                    if (roleCombobox.IsSelected)
                    {
                        result = await AddToRole(user, roleCombobox.Value);
                        if (result.Succeeded)
                        {
                            return Result<IdentityResult>.Success(result);
                        }
                        else
                        {
                            return Result<IdentityResult>.Failed(new List<ErrorResult>
                            {
                                new ErrorResult($"Пользователь создался, но не удалось добавить роль {roleCombobox.Value}")
                            }, result);
                        }
                    }
                }
            }
            return Result<IdentityResult>.Failed(new List<ErrorResult>
                {
                    new ErrorResult("Не удалось создать пользователя!")
                },
                result);
        }
        public async Task<Result> DeleteUser(EntityDto<long> input)
        {
            var user = await _userManager.FindByIdAsync(input.Id.ToString());
            if (user != null)
            {
                await _studentService.DeleteStudentByUserId(new EntityDto<long> { Id = user.Id });
                await _teacherService.DeleteTeacherByUserId(new EntityDto<long> { Id = user.Id });
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Result.Success();
                }
                var errors = new List<ErrorResult>();
                foreach (var error in result.Errors)
                {
                    errors.Add(new ErrorResult(error.Description));
                }
                return Result.Failed(errors);
            }
            return ErrorNotFoundUser(input.Id);
        }
        public async Task<Result<UserItemDto>> GetUserById(EntityDto<long> input)
        {
            var user = await _userManager.FindByIdAsync(input.Id.ToString());
            if (user != null)
            {
                var userDto = await MapEntityToEntityDto(user);
                return Result<UserItemDto>.Success(userDto);
            }

            return ErrorNotFoundUser(input.Id);
        }
        public async Task<Result<UserItemDto>> GetUserByClaims(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);
            if (user != null)
            {
                var userDto = await MapEntityToEntityDto(user);
                return Result<UserItemDto>.Success(userDto);
            }
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult("Пользователь не найден")
            });
        }
        public async Task<Result<ListResultDto<UserItemDto>>> GetUsers(GetUsersInput input)
        {
            List<UserItemDto> userDtos = new List<UserItemDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                bool userContains = false;
                foreach (var roleCombobox in input.RoleComboboxItems.Items)
                {
                    if (roleCombobox.IsSelected)
                    {
                        userContains = await _userManager.IsInRoleAsync(user, roleCombobox.Value);
                        if (userContains)
                        {
                            var userDto = await MapEntityToEntityDto(user);
                            var firstedUserDto = userDtos.FirstOrDefault(user => user.Id == userDto.Id);
                            if (firstedUserDto == null)
                            {
                                userDtos.Add(userDto);
                            }
                            
                        }
                    }
                }
                
            }

            return Result<ListResultDto<UserItemDto>>.Success(new ListResultDto<UserItemDto>(userDtos));
        }
        public async Task<Result<IdentityResult>> UpdateUserInfo(UpdateUserInfoInput input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId.ToString());
            if (user != null)
            {
                user.UserName = GetUpdatedOrStandartInfoString(user.UserName, input.UserName);
                user.Email = GetUpdatedOrStandartInfoString(user.Email, input.Email);
                user.FirstName = GetUpdatedOrStandartInfoString(user.FirstName, input.FirstName);
                user.LastName = GetUpdatedOrStandartInfoString(user.LastName, input.LastName);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    foreach (var roleCombobox in input.RoleComboboxItems.Items)
                    {
                        var isInRole = await _userManager.IsInRoleAsync(user, roleCombobox.Value);
                        if (roleCombobox.IsSelected)
                        {
                            if (!isInRole)
                            {
                                await AddToRole(user, roleCombobox.Value);
                            }
                        }
                        else
                        {
                            if (isInRole)
                            {
                                await RemoveFromRole(user, roleCombobox.Value);
                            }
                        }
                    }
                    return Result<IdentityResult>.Success(result);
                }
                return Result<IdentityResult>.Failed(new List<ErrorResult> 
                { 
                    new ErrorResult($"Не удалось обносить данные для пользователя под Id - {user.Id}")
                }, result);
            }
            return ErrorNotFoundUser(input.UserId);
        }
        public async Task<Result<IdentityResult>> UpdateUserPassword(UpdateUserPasswordInput input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId.ToString());
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
                if (result.Succeeded)
                {
                    return Result<IdentityResult>.Success(result);
                }
                return Result<IdentityResult>.Failed(new List<ErrorResult>
                {
                    new ErrorResult($"Не удалось обновить пароль для пользователя с Id - {user.Id}")
                }, result);
            }
            return ErrorNotFoundUser(input.UserId);
        }
        protected override async Task<UserItemDto> MapEntityToEntityDto(User entity)
        {
            var userDto = new UserItemDto();
            if (entity != null)
            {
                userDto.Id = entity.Id;
                userDto.UserName = entity.UserName;
                userDto.Email = entity.Email;
                userDto.FirstName = entity.FirstName;
                userDto.LastName = entity.LastName;
                var rolesForUser = await _userManager.GetRolesAsync(entity);
                var allRoles = await _roleManager.Roles.ToListAsync();
                var roles = (from role in allRoles
                             from roleUserName in rolesForUser
                             where role.Name == roleUserName
                             select role).ToList();
                var rolesDto = new List<RoleItemDto>();
                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        if (role != null)
                        {
                            rolesDto.Add(new RoleItemDto { Name = role.Name, LocalizedName = role.LocalizedName });
                        }
                    }
                }
                userDto.Roles = new ListResultDto<RoleItemDto>(rolesDto);
            }
            return userDto;
        }
        private async Task<IdentityResult> AddToRole(User user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            switch (role)
            {
                case RolesConsts.Student.Name:
                    await _studentService.CreateStudent(new CreateStudentInput { UserId = user.Id });
                    break;
                case RolesConsts.Teacher.Name:
                    await _teacherService.CreateTeacher(new CreateTeacherInput { UserId = user.Id });
                    break;
            }
            return result;
        }
        private async Task<IdentityResult> RemoveFromRole(User user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            switch (role)
            {
                case RolesConsts.Student.Name:
                    await _studentService.DeleteStudentByUserId(new EntityDto<long> { Id = user.Id });
                    break;
                case RolesConsts.Teacher.Name:
                    await _teacherService.DeleteTeacherByUserId(new EntityDto<long> { Id = user.Id });
                    break;
            }
            return result;
        }
        private Result ErrorNotFoundUser(long userId)
        {
            return ErrorByIdFormat(userId, "Не найден пользователь под Id - {0}");
        }
    }

}
