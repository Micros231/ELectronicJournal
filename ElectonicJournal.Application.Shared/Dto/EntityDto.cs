using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public EntityDto()
        {

        }
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }
    public class EntityDto : EntityDto<int>, IEntityDto<int>, IEntityDto
    {

    }
}
