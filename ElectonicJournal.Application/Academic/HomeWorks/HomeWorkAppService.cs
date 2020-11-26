using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Academic;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using ElectronicJournal.Extenstions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Academic.HomeWorks
{
    public class HomeWorkAppService : AppServiceBase<HomeWork, HomeWorkItemDto, long>, IHomeWorkAppService
    {
        private readonly IRepository<HomeWork, long> _homeWokrRepository;
        private readonly IAcademicSubjectAppService _subjectService;
        private readonly IStudyGroupAppService _studyGroupService;
        public HomeWorkAppService(
            IRepository<HomeWork, long> homeWokrRepository,
            IAcademicSubjectAppService subjectService,
            IStudyGroupAppService studyGroupService)
        {
            _homeWokrRepository = homeWokrRepository;
            _subjectService = subjectService;
            _studyGroupService = studyGroupService;
        }
        public async Task<Result> CreateHomeWork(CreateHomeWorkInput input)
        {
            var errorList = new List<ErrorResult>();
            if (input.EndDate.HasValue)
            {
                var homeWork = new HomeWork(input.AcademicSubjectId, input.StudyGroupId, input.EndDate.Value);
                homeWork.Description = input.Description;
                homeWork.HomeWorkData = input.HomeWorkData;
                await _homeWokrRepository.InsertAsync(homeWork);
                return Result.Success();
            }
            errorList.Add(new ErrorResult("Дата имеет не верный формат"));
            return Result.Failed(errorList);
        }

        public async Task<Result<ListResultDto<HomeWorkItemDto>>> GetHomeWorks(GetHomeWorksInput input)
        {
            var query = _homeWokrRepository.GetAll();

            var homeWorks = await query.ToListAsync();
            if (input.StudyGroupId.HasValue)
            {
                homeWorks = homeWorks.Where(homeWork => homeWork.StudyGroupId == input.StudyGroupId.Value).ToList();
            }
            if (input.AcademicSubjectId.HasValue)
            {
                homeWorks = homeWorks.Where(homeWork => homeWork.AcademicSubjectId == input.AcademicSubjectId.Value).ToList();
            }
            if (input.EndDate.HasValue)
            {
                homeWorks = homeWorks.Where(homeWork =>
                homeWork.EndDate.Month == input.EndDate.Value.Month && homeWork.EndDate.Year == input.EndDate.Value.Year).ToList();
            }
            var homeWorkDtos = new List<HomeWorkItemDto>();
            foreach (var homework in homeWorks)
            {
                var homeWorkDto = await MapEntityToEntityDto(homework);
                homeWorkDtos.Add(homeWorkDto);
            }
            return Result<ListResultDto<HomeWorkItemDto>>.Success(new ListResultDto<HomeWorkItemDto>(homeWorkDtos));
        }

        public async Task<Result> UpdateHomeWork(UpdateHomeWorkInput input)
        {
            var homeWork = await _homeWokrRepository.GetAsync(input.HomeWorkId);
            if (homeWork != null)
            {
                homeWork.Description = GetUpdatedOrStandartInfoString(homeWork.Description, input.Description);
                if (!homeWork.HomeWorkData.IsNullOrEmpty())
                {
                    homeWork.HomeWorkData = input.HomeWorkData;
                }
                await _homeWokrRepository.UpdateAsync(homeWork);
                return Result.Success();
            }
            return ErrorByIdFormat(input.HomeWorkId, "Домашнего задания с Id - {0} не существует");
        }

        protected override async Task<HomeWorkItemDto> MapEntityToEntityDto(HomeWork entity)
        {
            var homeWorkDto = new HomeWorkItemDto();
            if (entity != null)
            {
                homeWorkDto.Id = entity.Id;
                homeWorkDto.EndDate = entity.EndDate;
                homeWorkDto.Description = entity.Description;
                homeWorkDto.HomeWorkData = entity.HomeWorkData;
                var resultGetStudyGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(entity.StudyGroupId));
                if (resultGetStudyGroup.IsSuccessed)
                {
                    homeWorkDto.StudyGroup = resultGetStudyGroup.Value;
                }
                var resultGetAcademicSubject = await _subjectService.GetAcademicSubject(new EntityDto<long>(entity.AcademicSubjectId));
                if (resultGetAcademicSubject.IsSuccessed)
                {
                    homeWorkDto.AcademicSubject = resultGetAcademicSubject.Value;
                }
            }
            return homeWorkDto;
        }
    }
}
