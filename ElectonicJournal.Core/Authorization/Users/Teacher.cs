using ElectronicJournal.Core.Academic;
using ElectronicJournal.Core.Entities;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Authorization.Users
{
    public class Teacher : Entity<long>, IHasUserId<long>
    {
        public virtual long UserId { get; set; }
        public virtual long? AcademicSubjectId { get; set; }
        public virtual List<TeacherStudyGroup> TeacherStudyGroups { get; private set; }

        public Teacher()
        {
            TeacherStudyGroups = new List<TeacherStudyGroup>();
        }
    }
}
