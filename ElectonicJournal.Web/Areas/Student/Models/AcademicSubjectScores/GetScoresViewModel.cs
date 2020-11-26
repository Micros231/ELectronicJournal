using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Student.Models.AcademicSubjectScores
{
    public class GetScoresViewModel
    {
        public GetScoresInput Input { get; set; }
        public ListResultDto<ScoreItemDto> Value { get; set; }
        public List<AcademicSubjectSimpleItemDto> Subjects { get; set; }
        public List<DayItemDto> Days { get; set; }

        public GetScoresViewModel()
        {
            Input = new GetScoresInput();
            Value = new ListResultDto<ScoreItemDto>();
            Subjects = new List<AcademicSubjectSimpleItemDto>();
            Days = new List<DayItemDto>();
        }
    }
}
