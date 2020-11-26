using ElectronicJournal.Application.Authorization.Users.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Users
{
    public class EditPasswordUserViewModel
    {
        public string UserName { get; set; }
        public UpdateUserPasswordInput Input { get; set; }
        public string ReturnUrl { get; set; }

        public EditPasswordUserViewModel()
        {
            Input = new UpdateUserPasswordInput();
        }
    }
}
