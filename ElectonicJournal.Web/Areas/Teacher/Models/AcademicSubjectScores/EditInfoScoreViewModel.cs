using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.AcademicSubjectScores
{
    public class EditInfoScoreViewModel
    {
        public UpdateSocreInfoInput Input { get; set; }
        public string StudentName { get; set; }
        public AcademicSubjectSimpleItemDto AcademicSubject { get; set; }
        public string ReturnUrl { get; set; }

        public EditInfoScoreViewModel()
        {
            Input = new UpdateSocreInfoInput();
        }
    }
}
