using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    public interface IHasAcademicSubjectId<TPrimaryKey>
    {
        public TPrimaryKey AcademicSubjectId { get; set; }
    }
}
