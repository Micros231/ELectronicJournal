using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    public interface IHasNames
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
