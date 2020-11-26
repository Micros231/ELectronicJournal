using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public interface IEntityDto<T>
    {
        public T Id { get; set; }
    }
    public interface IEntityDto : IEntityDto<int>
    {

    }
}
