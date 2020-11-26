using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Teacher
{
    public class CreateTeacherInput : IHasUserIdDto<long>, IHasAcademicSubjectId<long?>
    {
        public long UserId { get; set; }
        public long? AcademicSubjectId { get; set; }
    }
}
