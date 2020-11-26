using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using ElectronicJournal.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.User
{
    public class CreateUserInput : IHasNames
    {
        [Required(ErrorMessage = "Необходимое поле")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Необходимое поле")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Необходимое поле")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ComboboxItemDto AdminRole { get; set; } = new ComboboxItemDto(RolesConsts.Admin.Name, RolesConsts.Admin.LocalizedName);
        public ComboboxItemDto TeacherRole { get; set; } = new ComboboxItemDto(RolesConsts.Teacher.Name, RolesConsts.Teacher.LocalizedName);
        public ComboboxItemDto StudentRole { get; set; } = new ComboboxItemDto(RolesConsts.Student.Name, RolesConsts.Student.LocalizedName);
        public ListResultDto<ComboboxItemDto> RoleComboboxItems { get; }
        public CreateUserInput()
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
