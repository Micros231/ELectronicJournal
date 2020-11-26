using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.AcademicSubjects
{
    public class CreateAcademicSubjectViewModel
    {
        public CreateAcademicSubjectInput Input { get; set; }
        public string ReturnUrl { get; set; }
    }
}
