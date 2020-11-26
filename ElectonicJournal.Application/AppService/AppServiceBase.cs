using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Entities;
using ElectronicJournal.Extenstions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.AppService
{
    public abstract class AppServiceBase : IAppService
    {
         
    }
    public abstract class AppServiceBase<TEntity, TEntityDto> : AppServiceBase
    {
        protected abstract Task<TEntityDto> MapEntityToEntityDto(TEntity entity);
        protected string GetUpdatedOrStandartInfoString(string standartedInfo, string updatedInfo)
        {
            return updatedInfo.IsNullOrEmpty() ? standartedInfo : updatedInfo;
        }
    }
    public abstract class AppServiceBase<TEntity, TEntityDto, TPrimaryKey> : AppServiceBase<TEntity, TEntityDto>
        where TEntity : IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected Result ErrorByIdFormat(TPrimaryKey id, string message)
        {
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult(string.Format(message, id))
            });
        }

    }
}
