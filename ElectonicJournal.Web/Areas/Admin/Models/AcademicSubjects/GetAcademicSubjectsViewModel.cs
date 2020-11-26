using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.AcademicSubjects
{
    public class GetAcademicSubjectsViewModel
    {
        public GetAcademicSubjectsInput Input { get; set; }
        public ListResultDto<AcademicSubjectItemDto> Value { get; set; }
        public GetAcademicSubjectsViewModel()
        {
            Input = new GetAcademicSubjectsInput();
            Value = new ListResultDto<AcademicSubjectItemDto>();
        }
    }
}
