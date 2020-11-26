using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using ElectronicJournal.Core.ManyToManyRelationships;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Core.Academic;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using Microsoft.AspNetCore.Identity;
using ElectronicJournal.Extenstions;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;

namespace ElectronicJournal.Application.Authorization.Users
{
    public class TeacherAppService : AppServiceBase<Teacher, TeacherItemDto, long>, ITeacherAppService
    {
        private readonly IRepository<Teacher, long> _teacherRespository;
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly UserManager<User> _userManager;
        public TeacherAppService(
            IRepository<Teacher, long> teacherRepository,
            IAcademicSubjectAppService academicSubjectService,
            UserManager<User> userManager)
        {
            _teacherRespository = teacherRepository;
            _academicSubjectService = academicSubjectService;
            _userManager = userManager;
        }
        public async Task<Result> CreateTeacher(CreateTeacherInput input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.UserId);
            if (teacher == null)
            {
                teacher = new Teacher
                {
                    UserId = input.UserId,
                    AcademicSubjectId = input.AcademicSubjectId
                };
                teacher = await _teacherRespository.InsertAsync(teacher);
                return Result.Success();
            }
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult($"Учитель с UserId -{input.UserId} уже существует")
            });
        }
        public async Task<Result> DeleteTeacherById(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.GetAsync(input.Id);
            if (teacher != null)
            {
                await _teacherRespository.DeleteAsync(teacher);
                return Result.Success();
            }
            return ErrorNotFoundTeacherWithId(input.Id);
        }
        public async Task<Result> DeleteTeacherByUserId(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.Id);
            if (teacher != null)
            {
                await _teacherRespository.DeleteAsync(teacher);
                return Result.Success();
            }
            return ErrorNotFoundTeacherWithUserId(input.Id);
        }
        public async Task<Result<TeacherItemDto>> GetTeacherById(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.GetAsync(input.Id);
            if (teacher != null)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                return Result<TeacherItemDto>.Success(teacherDto);
            }
            return ErrorNotFoundTeacherWithId(input.Id);
        }
        public async Task<Result<TeacherItemDto>> GetTeacherByUserId(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.Id);
            if (teacher != null)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                return Result<TeacherItemDto>.Success(teacherDto);
            }
            return ErrorNotFoundTeacherWithUserId(input.Id);
        }
        public async Task<Result<ListResultDto<TeacherItemDto>>> GetTeachers(GetTeachersInput input)
        {
            var query = _teacherRespository.GetAllIncluding(
                teach => teach.TeacherStudyGroups);
            if (input.AcademicSubjectId.HasValue)
            {
                query = query.Where(teach => teach.AcademicSubjectId == input.AcademicSubjectId.Value);
            }
            var teachers = await query.ToListAsync();
            if (input.StudyGroupId.HasValue)
            {
                teachers = teachers.Where(teacher => teacher.TeacherStudyGroups.Any(
                    teachStudyGroup => teachStudyGroup.StudyGroupId == input.StudyGroupId)).ToList();
            }

            var teacherDtos = new List<TeacherItemDto>();
            foreach (var teacher in teachers)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                var firstFindedDto = teacherDtos.FirstOrDefault(teach => teach.Id == teacherDto.Id);
                if (firstFindedDto == null)
                {
                    teacherDtos.Add(teacherDto);
                }
            }
            return Result<ListResultDto<TeacherItemDto>>.Success(new ListResultDto<TeacherItemDto>(teacherDtos));
        }
        public async Task<Result> UpdateTeacherInfo(UpdateTeacherInfoInput input)
        {
            var teacher = 
                await _teacherRespository.GetAllIncluding(teacher => teacher.TeacherStudyGroups)
                .FirstOrDefaultAsync(teacher => teacher.Id == input.TeacherId);
            if (teacher != null)
            {
                if (input.AcademicSubjectId.HasValue)
                {
                    var result =
                    await _academicSubjectService.GetAcademicSubject(
                        new EntityDto<long>(input.AcademicSubjectId.Value));
                    if (result.IsSuccessed)
                    {
                        teacher.AcademicSubjectId = input.AcademicSubjectId;
                    }
                }
                else
                {
                    teacher.AcademicSubjectId = null;
                }
                foreach (var combobox in input.StudyGroupComboboxes)
                {
                    var studyGroupId = long.Parse(combobox.Value);
                    var teacherStudyGroup =
                        teacher.TeacherStudyGroups.
                        FirstOrDefault(first => first.StudyGroupId == studyGroupId);
                    if (combobox.IsSelected)
                    {
                        if (teacherStudyGroup == null)
                        {
                            teacher.TeacherStudyGroups.Add(
                                new TeacherStudyGroup
                                {
                                    TeacherId = teacher.Id,
                                    StudyGroupId = studyGroupId
                                });
                        }
                    }
                    else
                    {
                        if (teacherStudyGroup != null)
                        {
                            teacher.TeacherStudyGroups.Remove(teacherStudyGroup);
                        }
                    }
                }
                await _teacherRespository.UpdateAsync(teacher);
                return Result.Success();
            }
            return ErrorNotFoundTeacherWithId(input.TeacherId);
        }
        protected override async Task<TeacherItemDto> MapEntityToEntityDto(Teacher entity)
        {
            var teacherDto = new TeacherItemDto();
            if (entity != null)
            {
                entity = await _teacherRespository.GetAllIncludingAndThenIncluding(
                    teacher => teacher.TeacherStudyGroups,
                    prop => prop.StudyGroup).FirstOrDefaultAsync(teacher => teacher.Id == entity.Id);
                teacherDto.Id = entity.Id;
                teacherDto.UserId = entity.UserId;
                var user = await _userManager.FindByIdAsync(entity.UserId.ToString());
                if (user != null)
                {
                    teacherDto.FirstName = user.FirstName;
                    teacherDto.LastName = user.LastName;
                }
                if (entity.AcademicSubjectId.HasValue)
                {
                    var resultGetAcademicSubject =
                        await _academicSubjectService.GetAcademicSubject(
                            new EntityDto<long>(entity.AcademicSubjectId.Value));
                    if (resultGetAcademicSubject.IsSuccessed)
                    {
                        teacherDto.AcademicSubject = resultGetAcademicSubject.Value;
                    }
                }
                teacherDto.StudyGroups = MapStudyGroups(entity.TeacherStudyGroups);
            }
            return teacherDto;
        }
        private List<StudyGroupSimpleItemDto> MapStudyGroups(List<TeacherStudyGroup> teacherStudyGroups)
        {
            var studyGroupDtos = new List<StudyGroupSimpleItemDto>();
            if (!teacherStudyGroups.IsNullOrEmpty())
            {
                var studyGroups = teacherStudyGroups.Select(teacherStudyGroup => teacherStudyGroup.StudyGroup);
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

        private Result ErrorNotFoundTeacherWithUserId(long userId)
        {
            return ErrorByIdFormat(userId, "Учителя с UserId - {0} не существует");
        }
        private Result ErrorNotFoundTeacherWithId(long teacherId)
        {
            return ErrorByIdFormat(teacherId, "Учителя с Id - {0} не существует");
        }


    }
}

    /*public class TeacherAppService : AppServiceBase<Teacher, TeacherItemDto, long>, ITeacherAppService
    {
        private readonly IRepository<Teacher, long> _teacherRespository;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly UserManager<User> _userManager;
        public TeacherAppService(
            IRepository<Teacher, long> teacherRepository,
            IStudyGroupAppService studyGroupService,
            IAcademicSubjectAppService academicSubjectService,
            UserManager<User> userManager)
        {
            _teacherRespository = teacherRepository;
            _studyGroupService = studyGroupService;
            _academicSubjectService = academicSubjectService;
            _userManager = userManager;
        }
        public async Task<Result> CreateTeacher(CreateTeacherInput input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.UserId);
            if (teacher == null)
            {
                teacher = new Teacher
                {
                    UserId = input.UserId,
                    AcademicSubjectId = input.AcademicSubjectId
                };
                teacher = await _teacherRespository.InsertAsync(teacher);
                foreach (var studyGroupId in input.StudyGroupIds)
                {
                    var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(studyGroupId));
                    if (result.IsSuccessed)
                    {
                        teacher.TeacherStudyGroups.Add(new TeacherStudyGroup
                        {
                            TeacherId = teacher.Id,
                            StudyGroupId = studyGroupId
                        });
                    }
                }
                await _teacherRespository.UpdateAsync(teacher);
                return Result.Success();
            }
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult($"Учитель с UserId -{input.UserId} уже существует")
            });
        }
        public async Task<Result> DeleteTeacherById(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.GetAsync(input.Id);
            if (teacher != null)
            {
                await _teacherRespository.DeleteAsync(teacher);
                return Result.Success();
            }
            return ErrorNotFoundTeacherWithId(input.Id);
        }
        public async Task<Result> DeleteTeacherByUserId(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.Id);
            if (teacher != null)
            {
                await _teacherRespository.DeleteAsync(teacher);
                return Result.Success();
            }
            return ErrorNotFoundTeacherWithUserId(input.Id);
        }
        public async Task<Result<TeacherItemDto>> GetTeacherById(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.GetAsync(input.Id);
            if (teacher != null)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                return Result<TeacherItemDto>.Success(teacherDto);
            }
            return ErrorNotFoundTeacherWithId(input.Id);
        }
        public async Task<Result<TeacherItemDto>> GetTeacherByUserId(EntityDto<long> input)
        {
            var teacher = await _teacherRespository.FirstOrDefaultAsync(teach => teach.UserId == input.Id);
            if (teacher != null)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                return Result<TeacherItemDto>.Success(teacherDto);
            }
            return ErrorNotFoundTeacherWithUserId(input.Id);
        }
        public async Task<Result<ListResultDto<TeacherItemDto>>> GetTeachers(GetTeachersInput input)
        {
            var query = _teacherRespository.GetAllIncluding(
                teach => teach.TeacherStudyGroups);
            if (input.AcademicSubjectId.HasValue)
            {
                query = query.Where(teach => teach.AcademicSubjectId == input.AcademicSubjectId.Value);
            }
            foreach (var studyGroupId in input.StudyGroupIds)
            {
                query = query.Where(teach => teach.TeacherStudyGroups.Any(
                    teachStudyGroup => teachStudyGroup.StudyGroupId == studyGroupId));
            }
            var teachers = await query.ToListAsync();
            var teacherDtos = new List<TeacherItemDto>();
            foreach (var teacher in teachers)
            {
                var teacherDto = await MapEntityToEntityDto(teacher);
                var firstFindedDto = teacherDtos.FirstOrDefault(teach => teach.Id == teacherDto.Id);
                if (firstFindedDto == null)
                {
                    teacherDtos.Add(teacherDto);
                }
            }
            return Result<ListResultDto<TeacherItemDto>>.Success(new ListResultDto<TeacherItemDto>(teacherDtos));
        }
        public async Task<Result> UpdateAcademicSubject(UpdateAcademicSubjectInput input)
        {
            if (input.AcademicSubjectId.HasValue)
            {
                var resultGetAcademicSubject =
                    await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(input.AcademicSubjectId.Value));
                if (resultGetAcademicSubject.IsSuccessed)
                {
                    var teacher = await _teacherRespository.GetAsync(input.TeacherId);
                    if (teacher != null)
                    {
                        teacher.AcademicSubjectId = input.AcademicSubjectId.Value;
                        await _teacherRespository.UpdateAsync(teacher);
                    }
                    return ErrorNotFoundTeacherWithId(input.TeacherId);
                }
                return Result.Failed(resultGetAcademicSubject.Errors.ToList());
            }
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult("Вы передали пустое Id учебного предмета")
            });
        }
        
        public async Task<Result> AddStudyGroupToTeacher(ChangeStudyGroupsInput input)
        {
            var teacher = await _teacherRespository.GetAsync(input.TeacherId);
            if (teacher != null)
            {
                var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(input.StudyGroupId));
                if (result.IsSuccessed)
                {
                    teacher.TeacherStudyGroups.Add(
                        new TeacherStudyGroup { StudyGroupId = input.StudyGroupId, TeacherId = input.TeacherId });
                    return Result.Success();
                }
                return Result.Failed(result.Errors.ToList());
            }
            return ErrorNotFoundTeacherWithId(input.TeacherId);
        }
        public async Task<Result> RemoveStudyGroupFromTeacher(ChangeStudyGroupsInput input)
        {
            var teacherIncludeStudyGroupQuery = _teacherRespository.GetAllIncluding(teach => teach.TeacherStudyGroups);
            var teacher = await teacherIncludeStudyGroupQuery.FirstOrDefaultAsync(teach => teach.Id == input.TeacherId);
            if (teacher != null)
            {
                var teacherStudyGroup = teacher.TeacherStudyGroups.FirstOrDefault(teachStudyGroup => 
                teachStudyGroup.StudyGroupId == input.StudyGroupId &&
                teachStudyGroup.TeacherId == input.TeacherId);
                if (teacherStudyGroup != null)
                {
                    teacher.TeacherStudyGroups.Remove(teacherStudyGroup);
                    await _teacherRespository.UpdateAsync(teacher);
                    return Result.Success();
                }
                return Result.Failed(new List<ErrorResult>
                {
                    new ErrorResult($"Учитель {teacher.Id} не причастен к группе {input.StudyGroupId} или такой группы не существует!")
                });
            }
            return ErrorNotFoundTeacherWithId(input.TeacherId);
        }
        protected override async Task<TeacherItemDto> MapEntityToEntityDto(Teacher entity)
        {
            var teacherDto = new TeacherItemDto();
            if (entity != null)
            {
                var query = _teacherRespository.GetAllIncluding(teach => teach.TeacherStudyGroups);
                entity = await query.FirstOrDefaultAsync(teach => teach.Id == entity.Id);
                teacherDto.Id = entity.Id;
                teacherDto.UserId = entity.UserId;
                var user = await _userManager.FindByIdAsync(entity.UserId.ToString());
                if (user != null)
                {
                    teacherDto.FirstName = user.FirstName;
                    teacherDto.LastName = user.LastName;
                }
                if (entity.AcademicSubjectId.HasValue)
                {
                    var resultGetAcademicSubject =
                        await _academicSubjectService.GetAcademicSubject(
                            new EntityDto<long>(entity.AcademicSubjectId.Value));
                    if (resultGetAcademicSubject.IsSuccessed)
                    {
                        teacherDto.AcademicSubject = resultGetAcademicSubject.Value;
                    }
                }
                var teacherStudyGroups = entity.TeacherStudyGroups;
                foreach (var teacherStudyGroup in teacherStudyGroups)
                {
                    var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(teacherStudyGroup.StudyGroupId));
                    if (result.IsSuccessed)
                    {
                        teacherDto.StudyGroups.Add(result.Value);
                    }

                }
            }
            return teacherDto;
        }
        private Result ErrorNotFoundTeacherWithUserId(long userId)
        {
            return ErrorByIdFormat(userId, "Учителя с UserId - {0} не существует");
        }
        private Result ErrorNotFoundTeacherWithId(long teacherId)
        {
            return ErrorByIdFormat(teacherId, "Учителя с Id - {0} не существует");
        }

        
    }*/

