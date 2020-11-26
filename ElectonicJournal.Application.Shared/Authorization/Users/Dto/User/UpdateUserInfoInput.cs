using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.User
{
    public class UpdateUserInfoInput : IHasUserIdDto<long>, IHasNames
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ComboboxItemDto AdminRole { get; set; } = 
            new ComboboxItemDto(RolesConsts.Admin.Name, RolesConsts.Admin.LocalizedName);
        public ComboboxItemDto TeacherRole { get; set; } = 
            new ComboboxItemDto(RolesConsts.Teacher.Name, RolesConsts.Teacher.LocalizedName);
        public ComboboxItemDto StudentRole { get; set; } = 
            new ComboboxItemDto(RolesConsts.Student.Name, RolesConsts.Student.LocalizedName);
        public ListResultDto<ComboboxItemDto> RoleComboboxItems { get; }
        public UpdateUserInfoInput()
        {
            RoleComboboxItems = new ListResultDto<ComboboxItemDto>(new List<ComboboxItemDto>
            {
                AdminRole,
                TeacherRole,
                StudentRole
            });
        }
    }
}
