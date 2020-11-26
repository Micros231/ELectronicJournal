using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.User
{
    public class UserItemDto : EntityDto<long>, IHasNames
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
        public ListResultDto<RoleItemDto> Roles { get; set; }

        public UserItemDto()
        {
            Roles = new ListResultDto<RoleItemDto>();
        }
    }
}
