using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.StudyGroups.Dto
{
    public class StudyGroupItemDto : StudyGroupSimpleItemDto
    {
        
        public List<TeacherSimpleItemDto> Teachers { get; set; }
        public List<AcademicSubjectSimpleItemDto> AcademicSubjects { get; set; }

        public StudyGroupItemDto()
        {
            Teachers = new List<TeacherSimpleItemDto>();
            AcademicSubjects = new List<AcademicSubjectSimpleItemDto>();
        }
    }
}
