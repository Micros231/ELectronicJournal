using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Authorization.Users.Dto.Teacher
{
    public class TeacherSimpleItemDto : EntityDto<long>, IHasUserIdDto<long>, IHasNames
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
    }
}
