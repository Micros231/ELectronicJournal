using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjects.Dto
{
    public class UpdateAcademicSubjectInput : IHasAcademicSubjectId<long>, IHasName
    {
        public string Name { get; set; }
        public long AcademicSubjectId { get; set; }
        public List<ComboboxItemDto> StudyGroupComboboxes { get; set; }
        public UpdateAcademicSubjectInput()
        {
            StudyGroupComboboxes = new List<ComboboxItemDto>();
        }
    }
}
