using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto.HasDto
{
    public interface IHasTeacherId<TPrimaryKey>
    {
        public TPrimaryKey TeacherId { get; set; }
    }
}
