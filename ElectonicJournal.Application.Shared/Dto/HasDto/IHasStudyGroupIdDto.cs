using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    public interface IHasStudyGroupIdDto<TPrimaryKey>
    {
        TPrimaryKey StudyGroupId { get; set; }
    }
}
