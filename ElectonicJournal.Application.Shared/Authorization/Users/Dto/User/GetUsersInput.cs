using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.User
{
    public class GetUsersInput
    {
        public ComboboxItemDto RoleAdmin { get; set; } = 
            new ComboboxItemDto(RolesConsts.Admin.Name, RolesConsts.Admin.LocalizedName);
        public ComboboxItemDto TeacherRole { get; set; } = 
            new ComboboxItemDto(RolesConsts.Teacher.Name, RolesConsts.Teacher.LocalizedName);
        public ComboboxItemDto StudentRole { get; set; } = 
            new ComboboxItemDto(RolesConsts.Student.Name, RolesConsts.Student.LocalizedName);
        public ListResultDto<ComboboxItemDto> RoleComboboxItems { get; }
        public GetUsersInput()
        {
            RoleComboboxItems = new ListResultDto<ComboboxItemDto>(new List<ComboboxItemDto>
            {
                RoleAdmin,
                TeacherRole,
                StudentRole
            });
        }
    }
}
