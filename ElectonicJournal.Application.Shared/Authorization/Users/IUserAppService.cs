using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Application.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Authorization.Users
{
    public interface IUserAppService : IAppService
    {
        Task<Result<ListResultDto<UserItemDto>>> GetUsers(GetUsersInput input);
        Task<Result<UserItemDto>> GetUserById(EntityDto<long> input);
        Task<Result<UserItemDto>> GetUserByClaims(ClaimsPrincipal user);
        Task<Result<IdentityResult>> CreateUser(CreateUserInput input);
        Task<Result> DeleteUser(EntityDto<long> input);
        Task<Result<IdentityResult>> UpdateUserInfo(UpdateUserInfoInput input);
        Task<Result<IdentityResult>> UpdateUserPassword(UpdateUserPasswordInput input);
    }
}
