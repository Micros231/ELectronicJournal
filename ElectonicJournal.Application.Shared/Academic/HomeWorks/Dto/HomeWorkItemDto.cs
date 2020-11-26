using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.HomeWorks.Dto
{
    public class HomeWorkItemDto : EntityDto<long>
    {
        public AcademicSubjectItemDto AcademicSubject { get; set; }
        public StudyGroupItemDto StudyGroup { get; set; }
        public string Description { get; set; }
        public byte[] HomeWorkData { get; set; }
        public DateTime EndDate { get; set; }
    }
}
