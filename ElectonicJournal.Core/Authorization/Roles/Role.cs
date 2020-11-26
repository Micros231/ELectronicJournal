using ElectronicJournal.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Core.Authorization.Roles
{
    public class Role : IdentityRole<long>, IEntity<long>
    {
        public string Description { get; set; }
        public string LocalizedName { get; set; }
        public Role()
        {

        }
        public Role(string name, string localizedName, string description)
        {
            Name = name;
            LocalizedName = localizedName;
            Description = description;
        }
    }
}
