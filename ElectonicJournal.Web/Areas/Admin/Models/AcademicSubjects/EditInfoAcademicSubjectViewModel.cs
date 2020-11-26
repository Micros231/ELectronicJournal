using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.AcademicSubjects
{
    public class EditInfoAcademicSubjectViewModel
    {
        public UpdateAcademicSubjectInput Input { get; set; }
        public string ReturnUrl { get; set; }
        public EditInfoAcademicSubjectViewModel()
        {
            Input = new UpdateAcademicSubjectInput();
        }
    }
}
