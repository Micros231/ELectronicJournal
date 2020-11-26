using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.StudyGroups.Dto
{
    public class GetStudyGroupsInput
    {
        public long? AcademicSubjectId { get; set; }
        public long? TeacherId { get; set; }

    }
}
