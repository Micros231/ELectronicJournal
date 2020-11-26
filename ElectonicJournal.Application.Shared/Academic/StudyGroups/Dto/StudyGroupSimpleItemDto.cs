using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.StudyGroups.Dto
{
    public class StudyGroupSimpleItemDto : EntityDto<long>, IHasName
    {
        public string Name { get; set; }
    }
}
