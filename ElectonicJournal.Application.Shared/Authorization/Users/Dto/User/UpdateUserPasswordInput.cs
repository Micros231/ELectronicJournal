using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.User
{
    public class UpdateUserPasswordInput : IHasUserIdDto<long>
    {
        public long UserId { get; set; }
        [Required(ErrorMessage = "Вы не ввели нынешний пароль.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Вы не ввели новый пароль.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
