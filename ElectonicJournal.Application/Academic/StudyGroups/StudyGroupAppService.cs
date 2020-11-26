using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Academic;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Extenstions;
using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using Microsoft.AspNetCore.Identity;
using ElectronicJournal.Core.Authorization.Users;

namespace ElectronicJournal.Application.Academic.StudyGroups
{
    public class StudyGroupAppService : AppServiceBase<StudyGroup, StudyGroupItemDto, long>, IStudyGroupAppService
    {
        private readonly IRepository<StudyGroup, long> _studyGroupRepository;
        private readonly UserManager<User> _userManager;
        public StudyGroupAppService(
            IRepository<StudyGroup, long> repository,
            UserManager<User> userManager)
        {
            _studyGroupRepository = repository;
            _userManager = userManager;
        }
        public async Task<Result> CreateStudyGroup(CreateStudyGroupInput input)
        {
            var studyGroup = new StudyGroup(input.Name);
            studyGroup = await _studyGroupRepository.InsertAsync(studyGroup);
            studyGroup = await _studyGroupRepository.
                GetAllIncluding(
                stdGrp => stdGrp.StudyGroupAcademicSubjects,
                stdGrp => stdGrp.TeacherStudyGroups).
                FirstOrDefaultAsync(stdGrp => stdGrp.Id == studyGroup.Id);
            UpdateComboboxValuesForAcademicSubjects(input.AcademicSubjectComboboxes, studyGroup);
            UpdateComboboxValuesForTeachers(input.TeacherComboboxes, studyGroup);
            await _studyGroupRepository.UpdateAsync(studyGroup);
            return Result.Success();
        }

        

        public async Task<Result> DeleteStudyGroup(EntityDto<long> input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.Id);
            if (studyGroup != null)
            {
                await _studyGroupRepository.DeleteAsync(studyGroup);
                return Result.Success();
            }
            return ErrorNotFoundStudyGroupWithId(input.Id);
        }

        public async Task<Result<StudyGroupItemDto>> GetStudyGroup(EntityDto<long> input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.Id);
            if (studyGroup != null)
            {
                var studyGroupDto = await MapEntityToEntityDto(studyGroup);
                return Result<StudyGroupItemDto>.Success(studyGroupDto);
            }
            return ErrorNotFoundStudyGroupWithId(input.Id);
        }

        public async Task<Result<ListResultDto<StudyGroupItemDto>>> GetStudyGroups(GetStudyGroupsInput input)
        {
            var query = _studyGroupRepository.GetAllIncluding(
                studyGroup => studyGroup.StudyGroupAcademicSubjects,
                studyGroup => studyGroup.TeacherStudyGroups);
            var studyGroups = await query.ToListAsync();
            if (input.AcademicSubjectId.HasValue || 
                input.TeacherId.HasValue)
            {
                studyGroups = studyGroups.
                    Where(studyGroup => studyGroup.StudyGroupAcademicSubjects.Any(
                        prop => prop.AcademicSubjectId == input.AcademicSubjectId) ||
                        studyGroup.TeacherStudyGroups.Any(
                        prop => prop.TeacherId == input.TeacherId)).ToList();
            }
            var studyGroupDtos = new List<StudyGroupItemDto>();
            foreach (var studyGroup in studyGroups)
            {
                var studyGroupDto = await MapEntityToEntityDto(studyGroup);
                studyGroupDtos.Add(studyGroupDto);
            }
            return Result<ListResultDto<StudyGroupItemDto>>.
                Success(new ListResultDto<StudyGroupItemDto>(studyGroupDtos));
        }

        public async Task<Result> UpdateStudyGroupInfo(UpdateStudyGroupInfo input)
        {
            var studyGroup = await _studyGroupRepository.
                GetAllIncluding(
                stdGrp => stdGrp.StudyGroupAcademicSubjects,
                stdGrp => stdGrp.TeacherStudyGroups).
                FirstOrDefaultAsync(stdGrp => stdGrp.Id == input.StudyGroupId);
            if (studyGroup != null)
            {
                studyGroup.Name = GetUpdatedOrStandartInfoString(studyGroup.Name, input.Name);
                UpdateComboboxValuesForAcademicSubjects(input.AcademicSubjectComboboxes, studyGroup);
                UpdateComboboxValuesForTeachers(input.TeacherComboboxes, studyGroup);
                await _studyGroupRepository.UpdateAsync(studyGroup);
                return Result.Success();
            }
            return ErrorNotFoundStudyGroupWithId(input.StudyGroupId);
        }

        protected override async Task<StudyGroupItemDto> MapEntityToEntityDto(StudyGroup entity)
        {
            var studyGroupDto = new StudyGroupItemDto();
            if (entity != null)
            {
                var entityAcademicSubjects = await _studyGroupRepository.GetAllIncludingAndThenIncluding(
                    studyGroup => studyGroup.StudyGroupAcademicSubjects,
                    prop => prop.AcademicSubject).FirstOrDefaultAsync(studyGroup => studyGroup.Id == entity.Id);
                var entityTeachers = await _studyGroupRepository.GetAllIncludingAndThenIncluding(
                    studyGroup => studyGroup.TeacherStudyGroups,
                    prop => prop.Teacher).FirstOrDefaultAsync(studyGroup => studyGroup.Id == entity.Id);
                studyGroupDto.Id = entity.Id;
                studyGroupDto.Name = entity.Name;
                studyGroupDto.AcademicSubjects = MapAcademicSubjects(entityAcademicSubjects.StudyGroupAcademicSubjects);
                studyGroupDto.Teachers = await MapTeachers(entityTeachers.TeacherStudyGroups);
            }
            return studyGroupDto;
        }
        private void UpdateComboboxValuesForAcademicSubjects(List<ComboboxItemDto> input, StudyGroup studyGroup)
        {
            foreach (var combobox in input)
            {
                var academicSubjectId = long.Parse(combobox.Value);
                var academicSubjectStudyGroup =
                    studyGroup.StudyGroupAcademicSubjects.
                    FirstOrDefault(first => first.AcademicSubjectId == academicSubjectId);
                if (combobox.IsSelected)
                {
                    if (academicSubjectStudyGroup == null)
                    {
                        studyGroup.StudyGroupAcademicSubjects.Add(
                            new StudyGroupAcademicSubject
                            {
                                AcademicSubjectId = academicSubjectId,
                                StudyGroupId = studyGroup.Id
                            });
                    }
                }
                else
                {
                    if (academicSubjectStudyGroup != null)
                    {
                        studyGroup.StudyGroupAcademicSubjects.Remove(academicSubjectStudyGroup);
                    }
                }
            }
        }
        private void UpdateComboboxValuesForTeachers(List<ComboboxItemDto> input, StudyGroup studyGroup)
        {
            foreach (var combobox in input)
            {
                var teacherId = long.Parse(combobox.Value);
                var teacherStudyGroup =
                    studyGroup.TeacherStudyGroups.
                    FirstOrDefault(first => first.TeacherId == teacherId);
                if (combobox.IsSelected)
                {
                    if (teacherStudyGroup == null)
                    {
                        studyGroup.TeacherStudyGroups.Add(
                            new TeacherStudyGroup
                            {
                                TeacherId = teacherId,
                                StudyGroupId = studyGroup.Id
                            });
                    }
                }
                else
                {
                    if (teacherStudyGroup != null)
                    {
                        studyGroup.TeacherStudyGroups.Remove(teacherStudyGroup);
                    }
                }
            }
        }
        private async Task<List<TeacherSimpleItemDto>> MapTeachers(List<TeacherStudyGroup> teacherStudyGroups)
        {
            var teacherDtos = new List<TeacherSimpleItemDto>();
            if (!teacherStudyGroups.IsNullOrEmpty())
            {
                var teachers = teacherStudyGroups.Select(select => select.Teacher);
                foreach (var teacher in teachers)
                {
                    var teacherDto = new TeacherSimpleItemDto();
                    teacherDto.Id = teacher.Id;
                    teacherDto.UserId = teacher.UserId;
                    var user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
                    if (user != null)
                    {
                        teacherDto.FirstName = user.FirstName;
                        teacherDto.LastName = user.LastName;
                    }
                    teacherDtos.Add(teacherDto);
                }
            }
            return teacherDtos;
        }

        private List<AcademicSubjectSimpleItemDto> MapAcademicSubjects(List<StudyGroupAcademicSubject> studyGroupAcademicSubjects)
        {
            var academicSubjectDtos = new List<AcademicSubjectSimpleItemDto>();
            if (!studyGroupAcademicSubjects.IsNullOrEmpty())
            {
                var academicSubjects = studyGroupAcademicSubjects.Select(select => select.AcademicSubject);
                foreach (var academicSubject in academicSubjects)
                {
                    var academicSubjectDto = new AcademicSubjectSimpleItemDto();
                    academicSubjectDto.Id = academicSubject.Id;
                    academicSubjectDto.Name = academicSubject.Name;
                    academicSubjectDtos.Add(academicSubjectDto);
                }
                
            }
            return academicSubjectDtos;
        }

        private Result ErrorNotFoundStudyGroupWithId(long studyGroupId)
        {
            return ErrorByIdFormat(studyGroupId, "Учебной группы с Id - {0} не существует");
        }
    }
    /*public class StudyGroupAppService : AppServiceBase<StudyGroup, StudyGroupItemDto, long>, IStudyGroupAppService
    {
        private readonly IRepository<StudyGroup, long> _studyGroupRepository;
        //private readonly ITeacherAppService _teacherService;
        //private readonly IAcademicSubjectAppService _academicSubjectService;
        public StudyGroupAppService(
            IRepository<StudyGroup, long> repository,
            ITeacherAppService teacherService,
            IAcademicSubjectAppService academicSubjectService)
        {
            _studyGroupRepository = repository;
            //_teacherService = teacherService;
            _academicSubjectService = academicSubjectService;
        }
        public async Task<Result> CreateStudyGroup(CreateStudyGroupInput input)
        {
            var studyGroup = new StudyGroup(input.Name);
            studyGroup = await _studyGroupRepository.InsertAsync(studyGroup);
            foreach (var teacherId in input.TeacherIds)
            {
                var result = await _teacherService.GetTeacherById(new EntityDto<long>(teacherId));
                if (result.IsSuccessed)
                {
                    studyGroup.TeacherStudyGroups.Add(new TeacherStudyGroup
                    {
                        StudyGroupId = studyGroup.Id,
                        TeacherId = teacherId
                    });
                }
            }
            foreach (var academicSubjectId in input.AcademicSubjectIds)
            {
                var result = await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(academicSubjectId));
                if (result.IsSuccessed)
                {
                    studyGroup.StudyGroupAcademicSubjects.Add(new StudyGroupAcademicSubject
                    {
                        StudyGroupId = studyGroup.Id,
                        AcademicSubjectId = academicSubjectId
                    });
                }
            }
            await _studyGroupRepository.UpdateAsync(studyGroup);
            return Result.Success();
        }

        public async Task<Result> DeleteStudyGroup(EntityDto<long> input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.Id);
            if (studyGroup != null)
            {
                await _studyGroupRepository.DeleteAsync(studyGroup);
                return Result.Success();
            }
            return ErrorNotFoundStudyGroupWithId(input.Id);
        }

        public async Task<Result<StudyGroupItemDto>> GetStudyGroup(EntityDto<long> input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.Id);
            if (studyGroup != null)
            {
                var studyGroupDto = await MapEntityToEntityDto(studyGroup);
                return Result<StudyGroupItemDto>.Success(studyGroupDto);
            }
            return ErrorNotFoundStudyGroupWithId(input.Id);
        }

        public async Task<Result<ListResultDto<StudyGroupItemDto>>> GetStudyGroups(GetStudyGroupsInput input)
        {
            var query = _studyGroupRepository.GetAllIncluding(
                studyGroup => studyGroup.StudyGroupAcademicSubjects,
                studyGroup => studyGroup.TeacherStudyGroups);
            foreach (var academicSubjectId in input.AcademicSubjectIds)
            {
                query = query.Where(studyGroup => studyGroup.StudyGroupAcademicSubjects.Any(
                    studyGroupAcadSubj => studyGroupAcadSubj.AcademicSubjectId == academicSubjectId));
            }
            foreach (var teacherId in input.TeqcherIds)
            {
                query = query.Where(studyGroup => studyGroup.TeacherStudyGroups.Any(
                    teacherStudyGroup => teacherStudyGroup.TeacherId == teacherId));
            }
            var studyGroups = await query.ToListAsync();
            var studyGroupDtos = new List<StudyGroupItemDto>();
            foreach (var studyGroup in studyGroups)
            {
                var studyGroupDto = await MapEntityToEntityDto(studyGroup);
                studyGroupDtos.Add(studyGroupDto);
            }
            return Result<ListResultDto<StudyGroupItemDto>>.
                Success(new ListResultDto<StudyGroupItemDto>(studyGroupDtos));
        }

        public async Task<Result> AddAcademicSubjectToStudyGroup(ChangeAcademicSubjectsInput input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.StudyGroupId);
            if (studyGroup != null)
            {
                var result = await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(input.AcademicSubjectId));
                if (result.IsSuccessed)
                {
                    studyGroup.StudyGroupAcademicSubjects.Add(new StudyGroupAcademicSubject
                    {
                        StudyGroupId = input.StudyGroupId,
                        AcademicSubjectId = input.AcademicSubjectId
                    });
                    return Result.Success();
                }
                return Result.Failed(result.Errors.ToList());
            }
            return ErrorNotFoundStudyGroupWithId(input.StudyGroupId);
        }
        public async Task<Result> RemoveAcademicSubjectFromStudyGroup(ChangeAcademicSubjectsInput input)
        {
            var studyGroupIncludedAcademicSubject = _studyGroupRepository.GetAllIncluding(
                studGroup => studGroup.StudyGroupAcademicSubjects);
            var studyGroup = await studyGroupIncludedAcademicSubject.FirstOrDefaultAsync(studGroup => studGroup.Id == input.StudyGroupId);
            if (studyGroup != null)
            {
                var studyGroupAcadSubj = studyGroup.StudyGroupAcademicSubjects.FirstOrDefault(studyGroupAcdSubj =>
                studyGroupAcdSubj.StudyGroupId == input.StudyGroupId &&
                studyGroupAcdSubj.AcademicSubjectId == input.AcademicSubjectId);
                if (studyGroupAcadSubj != null)
                {
                    studyGroup.StudyGroupAcademicSubjects.Remove(studyGroupAcadSubj);
                    await _studyGroupRepository.UpdateAsync(studyGroup);
                    return Result.Success();
                }
                return Result.Failed(new List<ErrorResult>
                {
                    new ErrorResult($"Учебная группа {input.StudyGroupId} " +
                    $"не причастна к учебному предмету {input.AcademicSubjectId} " +
                    $"или такого предмета не существует!")
                });
            }
            return ErrorNotFoundStudyGroupWithId(input.StudyGroupId);
        }
        public async Task<Result> AddTeacherToStudyGroup(ChangeTeachersInput input)
        {
            var studyGroup = await _studyGroupRepository.GetAsync(input.StudyGroupId);
            if (studyGroup != null)
            {
                var result = await _teacherService.GetTeacherById(new EntityDto<long>(input.TeacherId));
                if (result.IsSuccessed)
                {
                    studyGroup.TeacherStudyGroups.Add(new TeacherStudyGroup
                    {
                        StudyGroupId = input.StudyGroupId,
                        TeacherId = input.TeacherId
                    });
                    return Result.Success();
                }
                return Result.Failed(result.Errors.ToList());
            }
            return ErrorNotFoundStudyGroupWithId(input.StudyGroupId);
        }
        public async Task<Result> RemoveTeacherFromStudyGroup(ChangeTeachersInput input)
        {
            var studyGroupIncludedTeacher = _studyGroupRepository.GetAllIncluding(
                studGroup => studGroup.TeacherStudyGroups);
            var studyGroup = await studyGroupIncludedTeacher.FirstOrDefaultAsync(studGroup => studGroup.Id == input.StudyGroupId);
            if (studyGroup != null)
            {
                var studyGroupTeacher =
                    studyGroup.TeacherStudyGroups.FirstOrDefault(studyGroupTeach =>
                    studyGroupTeach.StudyGroupId == input.StudyGroupId &&
                    studyGroupTeach.TeacherId == input.TeacherId);
                if (studyGroupTeacher != null)
                {
                    studyGroup.TeacherStudyGroups.Remove(studyGroupTeacher);
                    await _studyGroupRepository.UpdateAsync(studyGroup);
                    return Result.Success();
                }
                return Result.Failed(new List<ErrorResult>
                {
                    new ErrorResult($"Учебная группа {input.StudyGroupId} " +
                    $"не причастна к учителю {input.TeacherId} " +
                    $"или такого учителя не существует!")
                });
            }
            return ErrorNotFoundStudyGroupWithId(input.StudyGroupId);
        }
        protected override async Task<StudyGroupItemDto> MapEntityToEntityDto(StudyGroup entity)
        {
            var studyGroupDto = new StudyGroupItemDto();
            if (entity != null)
            {
                var query =
                    _studyGroupRepository.GetAllIncluding(
                        studyGrp => studyGrp.StudyGroupAcademicSubjects,
                        studyGrp => studyGrp.TeacherStudyGroups);
                entity = await query.FirstOrDefaultAsync(studyGrp => studyGrp.Id == entity.Id);
                studyGroupDto.Id = entity.Id;
                studyGroupDto.Name = entity.Name;
                var teacherStudyGroups = entity.TeacherStudyGroups;
                var studyGroupAcadSubjs = entity.StudyGroupAcademicSubjects;
                foreach (var teacherStudyGroup in teacherStudyGroups)
                {
                    var result = await _teacherService.GetTeacherById(new EntityDto<long>(teacherStudyGroup.TeacherId));
                    if (result.IsSuccessed)
                    {
                        studyGroupDto.Teachers.Add(result.Value);
                    }
                }
                foreach (var studyGroupAcadSubj in studyGroupAcadSubjs)
                {
                    var result =
                        await _academicSubjectService.GetAcademicSubject(
                            new EntityDto<long>(studyGroupAcadSubj.AcademicSubjectId));
                    if (result.IsSuccessed)
                    {
                        studyGroupDto.AcademicSubjects.Add(result.Value);
                    }
                }
            }
            return studyGroupDto;
        }
        private Result ErrorNotFoundStudyGroupWithId(long studyGroupId)
        {
            return ErrorByIdFormat(studyGroupId, "Учебной группы с Id - {0} не существует");
        }


    }*/
}
