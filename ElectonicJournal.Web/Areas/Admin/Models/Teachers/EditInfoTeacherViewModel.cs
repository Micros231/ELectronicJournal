using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Teachers
{
    public class EditInfoTeacherViewModel
    {
        public string ReturnUrl { get; set; }
        public string FullName { get; set; }
        public UpdateTeacherInfoInput Input { get; set; }

        public EditInfoTeacherViewModel()
        {
            Input = new UpdateTeacherInfoInput();
        }
    }
}
