using ElectronicJournal.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Authorization.Users
{
    public class User : IdentityUser<long>, IEntity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
