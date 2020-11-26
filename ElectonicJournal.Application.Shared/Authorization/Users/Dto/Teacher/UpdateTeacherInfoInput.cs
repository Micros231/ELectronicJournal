using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Teacher
{
    public class UpdateTeacherInfoInput : IHasAcademicSubjectId<long?>, IHasTeacherId<long>
    {
        public long TeacherId { get; set; }
        public long? AcademicSubjectId { get; set; }
        public List<ComboboxItemDto> StudyGroupComboboxes { get; set; }

        public UpdateTeacherInfoInput()
        {
            StudyGroupComboboxes = new List<ComboboxItemDto>();
        }
    }
}
