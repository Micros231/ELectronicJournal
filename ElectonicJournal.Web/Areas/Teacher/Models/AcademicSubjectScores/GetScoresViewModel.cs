using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.AcademicSubjectScores
{
    public class GetScoresViewModel
    {
        public GetScoresInput Input { get; set; }
        public ListResultDto<ScoreItemDto> Value { get; set; }
        public List<StudentItemDto> Students { get; set; }
        public List<DayItemDto> Days { get; set; }
        public AcademicSubjectSimpleItemDto AcademicSubject { get; set; }

        public GetScoresViewModel()
        {
            Input = new GetScoresInput();
            Value = new ListResultDto<ScoreItemDto>();
            Students = new List<StudentItemDto>();
            Days = new List<DayItemDto>();
        }
    }
}
