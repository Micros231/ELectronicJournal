using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.AcademicSubjectScores
{
    public class CreateScoreViewModel
    {
        public string ReturnUrl { get; set; }
        public CreateScoreInput Input { get; set; }
        public string StudentName { get; set; }
        public AcademicSubjectSimpleItemDto AcademicSubject { get; set; }
    }
}
