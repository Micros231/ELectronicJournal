using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Academic.HomeWorks
{
    public interface IHomeWorkAppService : IAppService
    {
        Task<Result> CreateHomeWork(CreateHomeWorkInput input);
        Task<Result<ListResultDto<HomeWorkItemDto>>> GetHomeWorks(GetHomeWorksInput input);
        Task<Result> UpdateHomeWork(UpdateHomeWorkInput input);
    }
}
