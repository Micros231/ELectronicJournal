using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto
{
    public class ScoreItemDto : EntityDto<long>
    {
        public StudentItemDto Student { get; set; }
        public AcademicSubjectItemDto AcademicSubject { get; set; }
        public TeacherItemDto Teacher { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
