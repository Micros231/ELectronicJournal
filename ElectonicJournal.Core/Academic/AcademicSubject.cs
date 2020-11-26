using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.Core.Entities;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Academic
{
    public class AcademicSubject : Entity<long>
    {
        public virtual string Name { get; set; }
        public virtual List<StudyGroupAcademicSubject> StudyGroupAcademicSubjects { get; private set; }

        public AcademicSubject()
        {
            StudyGroupAcademicSubjects = new List<StudyGroupAcademicSubject>();
        }
        public AcademicSubject(string name) : this()
        {
            Name = name;
        }
    }
}
