using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Students
{
    public class EditInfoStudentViewModel
    {
        public string ReturnUrl { get; set; }
        public string FullName { get; set; }
        public UpdateStudentInfoInput Input { get; set; }
        public EditInfoStudentViewModel()
        {
            Input = new UpdateStudentInfoInput();
        }
    }
}
