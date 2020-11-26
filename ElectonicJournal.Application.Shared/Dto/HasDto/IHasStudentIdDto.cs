using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    interface IHasStudentIdDto<TPrimaryKey>
    {
        TPrimaryKey StudentId { get; set; }
    }
}
