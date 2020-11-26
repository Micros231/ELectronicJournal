using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.StudyGroups.Dto
{
    public class UpdateStudyGroupInfo : IHasStudyGroupIdDto<long>, IHasName
    {
        public string Name { get; set; }
        public long StudyGroupId { get; set; }

        public List<ComboboxItemDto> AcademicSubjectComboboxes { get; set; }
        public List<ComboboxItemDto> TeacherComboboxes { get; set; }
        public UpdateStudyGroupInfo()
        {
            AcademicSubjectComboboxes = new List<ComboboxItemDto>();
            TeacherComboboxes = new List<ComboboxItemDto>();
        }
    }
}
