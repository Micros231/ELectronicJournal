using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.StudyGroups.Dto
{
    public class CreateStudyGroupInput : IHasName
    {
        public string Name { get; set; }
        public List<ComboboxItemDto> AcademicSubjectComboboxes { get; set; }
        public List<ComboboxItemDto> TeacherComboboxes { get; set; }

        public CreateStudyGroupInput()
        {
            AcademicSubjectComboboxes = new List<ComboboxItemDto>();
            TeacherComboboxes = new List<ComboboxItemDto>();
        }
    }
}
