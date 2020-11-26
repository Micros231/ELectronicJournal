using ElectronicJournal.Core.Entities;
using System;

namespace ElectronicJournal.Core.Academic
{
    public class HomeWork : Entity<long>
    {
        public virtual long AcademicSubjectId { get; private set; }
        public virtual long StudyGroupId { get; private set; }
        public virtual byte[] HomeWorkData { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime EndDate { get; private set; }

        public HomeWork()
        {

        }
        public HomeWork(long academicSubjectId, long studyGroupId, DateTime endDate)
        {
            AcademicSubjectId = academicSubjectId;
            StudyGroupId = studyGroupId;
            EndDate = endDate;
        }
    }
}
