using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Teacher
{
    public class GetTeachersInput : IHasAcademicSubjectId<long?>
    {
        public long? StudyGroupId { get; set; }
        public long? AcademicSubjectId { get; set; }

    }
}
