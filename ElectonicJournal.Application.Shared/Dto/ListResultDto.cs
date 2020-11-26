﻿using System.Collections.Generic;

namespace ElectronicJournal.Application.Dto
{
    public class ListResultDto<T> : IListResult<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public ListResultDto()
        {

        }
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }

        
    }
}
