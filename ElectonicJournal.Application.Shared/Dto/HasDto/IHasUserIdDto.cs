using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    public interface IHasUserIdDto<TPrimaryKey> 
    {
        TPrimaryKey UserId { get; set; }
    }

}
