using ElectronicJournal.Core.Academic;
using ElectronicJournal.Core.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.ManyToManyRelationships
{
    public class TeacherStudyGroup
    {
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public long StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }
}
