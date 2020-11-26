using ElectronicJournal.Core.Academic;
using ElectronicJournal.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Authorization.Users
{
    public class Student : Entity<long>, IHasUserId<long>
    {
        public virtual long UserId { get; set; }
        public virtual long? StudyGroupId { get; set; }
    }
}
