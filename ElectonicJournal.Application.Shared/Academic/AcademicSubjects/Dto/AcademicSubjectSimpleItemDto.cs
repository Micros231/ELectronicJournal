using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjects.Dto
{
    public class AcademicSubjectSimpleItemDto : EntityDto<long>, IHasName
    {
        public string Name { get; set; }
    }
}
