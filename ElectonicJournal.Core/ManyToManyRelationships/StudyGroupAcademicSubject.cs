using ElectronicJournal.Core.Academic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.ManyToManyRelationships
{
    public class StudyGroupAcademicSubject
    {
        public virtual long StudyGroupId { get; set; }
        public virtual StudyGroup StudyGroup { get; set; }

        public virtual long AcademicSubjectId { get; set; }
        public virtual AcademicSubject AcademicSubject { get; set; }
    }
}
