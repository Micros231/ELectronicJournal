using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Authorization.Users
{
    public interface IHasUserId<TPrimaryKey>
    {
        public TPrimaryKey UserId { get; set; }
    }
    public interface IHasUserId : IHasUserId<int>
    {

    }
}
