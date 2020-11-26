using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
