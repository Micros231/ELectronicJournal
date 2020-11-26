using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjects.Dto
{
    public class AcademicSubjectItemDto : AcademicSubjectSimpleItemDto
    {
        public List<StudyGroupSimpleItemDto> StudyGroups { get; set; }
        public AcademicSubjectItemDto()
        {
            StudyGroups = new List<StudyGroupSimpleItemDto>();
        }

    }
}
