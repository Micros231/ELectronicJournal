using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Student
{
    public class CreateStudentInput : IHasUserIdDto<long>, IHasStudyGroupIdDto<long?>
    {
        public long UserId { get; set; }
        public long? StudyGroupId { get; set; }
    }
}
