using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
