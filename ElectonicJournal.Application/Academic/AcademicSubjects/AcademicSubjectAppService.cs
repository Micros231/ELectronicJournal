using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Academic;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Extenstions;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;

namespace ElectronicJournal.Application.Academic.AcademicSubjects
{
    public class AcademicSubjectAppService : AppServiceBase<AcademicSubject, AcademicSubjectItemDto, long>, IAcademicSubjectAppService
    {
        private readonly IRepository<AcademicSubject, long> _academicSubjectRepository;
        public AcademicSubjectAppService(
            IRepository<AcademicSubject, long> academicSubjectRepository)
        {
            _academicSubjectRepository = academicSubjectRepository;
        }
        public async Task<Result> CreateAcademicSubject(CreateAcademicSubjectInput input)
        {
            var academicSubject = new AcademicSubject(input.Name);
            academicSubject = await _academicSubjectRepository.InsertAsync(academicSubject);
            academicSubject = await _academicSubjectRepository.
                GetAllIncluding(acadSubj => acadSubj.StudyGroupAcademicSubjects).
                FirstOrDefaultAsync(acadSubj => acadSubj.Id == academicSubject.Id);
            UpdateComboboxeValuesForStudyGroups(input.StudyGroupComboboxes, academicSubject);
            await _academicSubjectRepository.UpdateAsync(academicSubject);
            return Result.Success();
        }

        

        public async Task<Result> DeleteAcademicSubject(EntityDto<long> input)
        {
            var academicSubject = await _academicSubjectRepository.GetAsync(input.Id);
            if (academicSubject != null)
            {
                await _academicSubjectRepository.DeleteAsync(academicSubject);
                return Result.Success();
            }
            return ErrorNotFoundAcademicSubjectWithId(input.Id);
        }

        public async Task<Result<AcademicSubjectItemDto>> GetAcademicSubject(EntityDto<long> input)
        {
            var academicSubject = await _academicSubjectRepository.GetAsync(input.Id);
            if (academicSubject != null)
            {
                var academicSubjectDto = await MapEntityToEntityDto(academicSubject);
                return Result<AcademicSubjectItemDto>.Success(academicSubjectDto);
            }
            return ErrorNotFoundAcademicSubjectWithId(input.Id);
        }

        public async Task<Result<ListResultDto<AcademicSubjectItemDto>>> GetAcademicSubjects(GetAcademicSubjectsInput input)
        {
            var query = _academicSubjectRepository.GetAllIncluding(
                acadSubj => acadSubj.StudyGroupAcademicSubjects);
            var academicSubjects = await query.ToListAsync();
            if (input.StudyGroupId.HasValue)
            {
                academicSubjects = academicSubjects.Where(acadSubj => acadSubj.StudyGroupAcademicSubjects.Any(
                    prop => prop.StudyGroupId == input.StudyGroupId)).ToList();
            }
            var academicSubjectDtos = new List<AcademicSubjectItemDto>();
            foreach (var academicSubject in academicSubjects)
            {
                var academicSubjectDto = await MapEntityToEntityDto(academicSubject);
                academicSubjectDtos.Add(academicSubjectDto);
            }
            return Result<ListResultDto<AcademicSubjectItemDto>>.Success(
                new ListResultDto<AcademicSubjectItemDto>(academicSubjectDtos));
        }

        public async Task<Result> UpdateAcademicSubjectInfo(UpdateAcademicSubjectInput input)
        {
            var academicSubject =
                await _academicSubjectRepository.
                GetAllIncluding(acadSubj => acadSubj.StudyGroupAcademicSubjects).
                FirstOrDefaultAsync(acadSubj => acadSubj.Id == input.AcademicSubjectId);
            if (academicSubject != null)
            {
                academicSubject.Name = GetUpdatedOrStandartInfoString(academicSubject.Name, input.Name);
                UpdateComboboxeValuesForStudyGroups(input.StudyGroupComboboxes, academicSubject);
                await _academicSubjectRepository.UpdateAsync(academicSubject);
                return Result.Success();
            }
            return ErrorNotFoundAcademicSubjectWithId(input.AcademicSubjectId);
        }

        protected override async Task<AcademicSubjectItemDto> MapEntityToEntityDto(AcademicSubject entity)
        {
            var academicSubjectDto = new AcademicSubjectItemDto();
            if (entity != null)
            {
                entity = await _academicSubjectRepository.GetAllIncludingAndThenIncluding(
                    acadSubj => acadSubj.StudyGroupAcademicSubjects,
                    prop => prop.StudyGroup).FirstOrDefaultAsync(acadSubj => acadSubj.Id == entity.Id);
                academicSubjectDto.Id = entity.Id;
                academicSubjectDto.Name = entity.Name;
                academicSubjectDto.StudyGroups = MapStudyGroups(entity.StudyGroupAcademicSubjects);
            }
            return academicSubjectDto;
        }
        private void UpdateComboboxeValuesForStudyGroups(List<ComboboxItemDto> input, AcademicSubject academicSubject)
        {
            foreach (var combobox in input)
            {
                var studyGroupId = long.Parse(combobox.Value);
                var academicSubjectStudyGroup =
                    academicSubject.StudyGroupAcademicSubjects.
                    FirstOrDefault(first => first.StudyGroupId == studyGroupId);
                if (combobox.IsSelected)
                {
                    if (academicSubjectStudyGroup == null)
                    {
                        academicSubject.StudyGroupAcademicSubjects.Add(
                            new StudyGroupAcademicSubject
                            {
                                AcademicSubjectId = academicSubject.Id,
                                StudyGroupId = studyGroupId
                            });
                    }
                }
                else
                {
                    if (academicSubjectStudyGroup != null)
                    {
                        academicSubject.StudyGroupAcademicSubjects.Remove(academicSubjectStudyGroup);
                    }
                }
            }
        }
        private List<StudyGroupSimpleItemDto> MapStudyGroups(List<StudyGroupAcademicSubject> studyGroupAcademicSubjects)
        {
            var studyGroupDtos = new List<StudyGroupSimpleItemDto>();
            if (!studyGroupAcademicSubjects.IsNullOrEmpty())
            {
                var studyGroups = studyGroupAcademicSubjects.Select(select => select.StudyGroup);
                foreach (var studyGroup in studyGroups)
                {
                    var studyGroupDto = new StudyGroupSimpleItemDto();
                    studyGroupDto.Name = studyGroup.Name;
                    studyGroupDto.Id = studyGroup.Id;
                    studyGroupDtos.Add(studyGroupDto);
                }
            }
            return studyGroupDtos;
        }

        private Result ErrorNotFoundAcademicSubjectWithId(long academicSubjectId)
        {
            return ErrorByIdFormat(academicSubjectId, "Учебного предмета с Id - {0} не существует");
        }
    }
}

/*public class AcademicSubjectAppService : AppServiceBase<AcademicSubject, AcademicSubjectItemDto, long>, IAcademicSubjectAppService
    {
        private readonly IRepository<AcademicSubject, long> _academicSubjectRepository;
        private readonly IStudyGroupAppService _studyGroupService;
        public AcademicSubjectAppService(
            IRepository<AcademicSubject, long> academicSubjectRepository,
            IStudyGroupAppService studyGroupService)
        {
            _academicSubjectRepository = academicSubjectRepository;
            _studyGroupService = studyGroupService;
        }
        public async Task<Result> CreateAcademicSubject(CreateAcademicSubjectInput input)
        {
            var academicSubject = new AcademicSubject(input.Name);
            academicSubject = await _academicSubjectRepository.InsertAsync(academicSubject);
            foreach (var studyGroupId in input.StudyGroupIds)
            {
                var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(studyGroupId));
                if (result.IsSuccessed)
                {
                    academicSubject.StudyGroupAcademicSubjects.Add(new StudyGroupAcademicSubject
                    {
                        AcademicSubjectId = academicSubject.Id,
                        StudyGroupId = studyGroupId
                    });
                }
            }
            await _academicSubjectRepository.UpdateAsync(academicSubject);
            return Result.Success();
        }

        public async Task<Result> DeleteAcademicSubject(EntityDto<long> input)
        {
            var academicSubject = await _academicSubjectRepository.GetAsync(input.Id);
            if (academicSubject != null)
            {
                await _academicSubjectRepository.DeleteAsync(academicSubject);
                return Result.Success();
            }
            return ErrorNotFoundAcademicSubjectWithId(input.Id);
        }

        public async Task<Result<AcademicSubjectItemDto>> GetAcademicSubject(EntityDto<long> input)
        {
            var academicSubject = await _academicSubjectRepository.GetAsync(input.Id);
            if (academicSubject != null)
            {
                var academicSubjectDto = await MapEntityToEntityDto(academicSubject);
                return Result<AcademicSubjectItemDto>.Success(academicSubjectDto);
            }
            return ErrorNotFoundAcademicSubjectWithId(input.Id);
        }

        public async Task<Result<ListResultDto<AcademicSubjectItemDto>>> GetAcademicSubjects(GetAcademicSubjectsInput input)
        {
            var query = _academicSubjectRepository.GetAllIncluding(
                acadSubj => acadSubj.StudyGroupAcademicSubjects);
            foreach (var studyGroupId in input.StudyGroupIds)
            {
                query = query.Where(acadSubj => acadSubj.StudyGroupAcademicSubjects.Any(
                    studGroupAcadSubj => studGroupAcadSubj.StudyGroupId == studyGroupId));
            }
            var academicSubjects = await query.ToListAsync();
            var academicSubjectDtos = new List<AcademicSubjectItemDto>();
            foreach (var academicSubject in academicSubjects)
            {
                var academicSubjectDto = await MapEntityToEntityDto(academicSubject);
                academicSubjectDtos.Add(academicSubjectDto);
            }
            return Result<ListResultDto<AcademicSubjectItemDto>>.Success(
                new ListResultDto<AcademicSubjectItemDto>(academicSubjectDtos));
        }
        public async Task<Result> AddStudyGroupToAcademicSubject(ChangeStudyGroupsInput input)
        {
            var academicSubject = await _academicSubjectRepository.GetAsync(input.AcademicSubjectId);
            if (academicSubject != null)
            {
                var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(input.StudyGroupId));
                if (result.IsSuccessed)
                {
                    academicSubject.StudyGroupAcademicSubjects.Add(new StudyGroupAcademicSubject
                    {
                        StudyGroupId = input.StudyGroupId,
                        AcademicSubjectId = input.AcademicSubjectId
                    });
                    return Result.Success();
                }
                return Result.Failed(result.Errors.ToList());
            }
            return ErrorNotFoundAcademicSubjectWithId(input.AcademicSubjectId);
        }
        public async Task<Result> RemoveStudyGroupFromAcademicSubject(ChangeStudyGroupsInput input)
        {
            var academicSubjectIncludedStudyGroup = _academicSubjectRepository.GetAllIncluding(
                acadSubj => acadSubj.StudyGroupAcademicSubjects);
            var academicSubject = await academicSubjectIncludedStudyGroup.FirstOrDefaultAsync(acadSubj => acadSubj.Id == input.AcademicSubjectId);
            if (academicSubject != null)
            {
                var studyGroupAcadSubj = academicSubject.StudyGroupAcademicSubjects.FirstOrDefault(studyGroupAcdSubj => 
                studyGroupAcdSubj.StudyGroupId == input.StudyGroupId &&
                studyGroupAcdSubj.AcademicSubjectId == input.AcademicSubjectId);
                if (studyGroupAcadSubj != null)
                {
                    academicSubject.StudyGroupAcademicSubjects.Remove(studyGroupAcadSubj);
                    await _academicSubjectRepository.UpdateAsync(academicSubject);
                    return Result.Success();
                }
                return Result.Failed(new List<ErrorResult>
                {
                    new ErrorResult($"Учебный пердмет {input.AcademicSubjectId} " +
                    $"не причастна к учебной группе { input.StudyGroupId} " +
                    $"или такой группы не существует!")
                });
            }
            return ErrorNotFoundAcademicSubjectWithId(input.AcademicSubjectId);
        }

        protected override async Task<AcademicSubjectItemDto> MapEntityToEntityDto(AcademicSubject entity)
        {
            var academicSubjectDto = new AcademicSubjectItemDto();
            if (entity != null)
            {
                var query = _academicSubjectRepository.GetAllIncluding(
                    acadSubj => acadSubj.StudyGroupAcademicSubjects);
                entity = await _academicSubjectRepository.FirstOrDefaultAsync(acadSubj => acadSubj.Id == entity.Id);
                academicSubjectDto.Id = entity.Id;
                academicSubjectDto.Name = entity.Name;
                var studyGroupAcadSubjs = entity.StudyGroupAcademicSubjects;
                foreach (var studyGroupAcadSubj in studyGroupAcadSubjs)
                {
                    var result =
                        await _studyGroupService.GetStudyGroup(
                            new EntityDto<long>(studyGroupAcadSubj.StudyGroupId));
                    if (result.IsSuccessed)
                    {
                        academicSubjectDto.StudyGroups.Add(result.Value);
                    }
                }
            }
            return academicSubjectDto;
        }

        private Result ErrorNotFoundAcademicSubjectWithId(long academicSubjectId)
        {
            return ErrorByIdFormat(academicSubjectId, "Учебного предмета с Id - {0} не существует");
        }
    }
*/