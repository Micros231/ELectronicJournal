using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Student
{
    public class GetStudentsInput : IHasStudyGroupIdDto<long?>
    {
        public long? StudyGroupId { get; set; }
    }
}
