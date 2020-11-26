using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Core.Entities;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Academic
{
    public class StudyGroup : Entity<long>
    {
        public virtual string Name { get; set; }
        public virtual List<TeacherStudyGroup> TeacherStudyGroups { get; private set; }
        public virtual List<StudyGroupAcademicSubject> StudyGroupAcademicSubjects { get; private set; }

        public StudyGroup()
        {
            TeacherStudyGroups = new List<TeacherStudyGroup>();
            StudyGroupAcademicSubjects = new List<StudyGroupAcademicSubject>();
        }
        public StudyGroup(string name) : this()
        {
            Name = name;
        }
    }
}
