using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Teacher
{
    public class TeacherItemDto : TeacherSimpleItemDto
    {
        public AcademicSubjectSimpleItemDto AcademicSubject { get; set; }
        public List<StudyGroupSimpleItemDto> StudyGroups { get; set; }

        public TeacherItemDto()
        {
            StudyGroups = new List<StudyGroupSimpleItemDto>();
        }

        
    }
}
