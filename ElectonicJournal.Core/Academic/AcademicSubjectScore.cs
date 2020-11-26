using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Core.Academic
{
    public class AcademicSubjectScore : Entity<long>
    {
        public virtual long StudentId { get; private set; }
        public virtual long AcademicSubjectId { get; private set; }
        public virtual long TeacherId { get; private set; }
        [Range(0,5)]
        public virtual int Score { get; set; }
        public virtual DateTime Date { get; private set; }
        public AcademicSubjectScore()
        {

        }
        public AcademicSubjectScore(long studentId, long teacherId, long academicSubjectId, int score, DateTime date)
        {
            StudentId = studentId;
            TeacherId = teacherId;
            AcademicSubjectId = academicSubjectId;
            Score = score;
            Date = date;
        }

    }
}
