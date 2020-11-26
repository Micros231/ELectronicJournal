using ElectronicJournal.Application.Authorization.Users.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Users
{
    public class CreateUserViewModel
    {
        public CreateUserInput Input { get; set; }
        public string ReturnUrl { get; set; }
    }
}
