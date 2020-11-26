using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Authorization.Users
{
    public class StudentAppService : AppServiceBase<Student, StudentItemDto, long>, IStudentAppService
    {
        private readonly IRepository<Student, long> _studentRespository;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly UserManager<User> _userManager;
        public StudentAppService(
            IRepository<Student, long> studentRespository,
            IStudyGroupAppService studyGroupService,
            UserManager<User> userManager)
        {
            _studentRespository = studentRespository;
            _studyGroupService = studyGroupService;
            _userManager = userManager;
        }

        public async Task<Result> CreateStudent(CreateStudentInput input)
        {
            var student = await _studentRespository.FirstOrDefaultAsync(stud => stud.UserId == input.UserId);
            if (student == null)
            {
                student = new Student
                {
                    UserId = input.UserId,
                    StudyGroupId = input.StudyGroupId
                };
                await _studentRespository.InsertAsync(student);
                return Result.Success();
            }
            return Result.Failed(new List<ErrorResult>
            {
                new ErrorResult($"Студент с UserId - {input.UserId} уже существует")
            });
        }

        public async Task<Result> DeleteStudentById(EntityDto<long> input)
        {
            var student = await _studentRespository.GetAsync(input.Id);
            if (student != null)
            {
                await _studentRespository.DeleteAsync(student);
                return Result.Success();
            }
            return ErrorNotFoundStudentWithId(input.Id);
        }

        public async Task<Result> DeleteStudentByUserId(EntityDto<long> input)
        {
            var student = await _studentRespository.FirstOrDefaultAsync(stud => stud.UserId == input.Id);
            if (student != null)
            {
                await _studentRespository.DeleteAsync(student);
                return Result.Success();
            }
            return ErrorNotFoundStudentWithUserId(input.Id);
        }

        public async Task<Result<StudentItemDto>> GetStudentById(EntityDto<long> input)
        {
            var student = await _studentRespository.GetAsync(input.Id);
            if (student != null)
            {
                var studentDto = await MapEntityToEntityDto(student);
                return Result<StudentItemDto>.Success(studentDto);
            }
            return ErrorNotFoundStudentWithId(input.Id);
        }

        public async Task<Result<StudentItemDto>> GetStudentByUserId(EntityDto<long> input)
        {
            var student = await _studentRespository.FirstOrDefaultAsync(stud => stud.UserId == input.Id);
            if (student != null)
            {
                var studentDto = await MapEntityToEntityDto(student);
                return Result<StudentItemDto>.Success(studentDto);
            }
            return ErrorNotFoundStudentWithUserId(input.Id);
        }

        public async Task<Result<ListResultDto<StudentItemDto>>> GetStudents(GetStudentsInput input)
        {
            var query = _studentRespository.GetAll();
            if (input.StudyGroupId.HasValue)
            {
                query = query.Where(student => student.StudyGroupId == input.StudyGroupId.Value);
            }
            var students = await query.ToListAsync();
            var studentDtos = new List<StudentItemDto>();
            foreach (var student in students)
            {
                if (student != null)
                {
                    var studentDto = await MapEntityToEntityDto(student);
                    studentDtos.Add(studentDto);
                }
            }
            return Result<ListResultDto<StudentItemDto>>.Success(new ListResultDto<StudentItemDto>(studentDtos));
        }
        public async Task<Result> UpdateStudentInfo(UpdateStudentInfoInput input)
        {
            var student = await _studentRespository.GetAsync(input.StudentId);
            if (student != null)
            {
                if (input.StudyGroupId.HasValue)
                {
                    var result =
                        await _studyGroupService.GetStudyGroup(
                            new EntityDto<long> { Id = input.StudyGroupId.Value });
                    if (result.IsSuccessed)
                    {
                        student.StudyGroupId = input.StudyGroupId.Value;
                        
                    }
                }
                else
                {
                    student.StudyGroupId = null;
                }
                await _studentRespository.UpdateAsync(student);
                return Result.Success();
            }
            return ErrorNotFoundStudentWithId(input.StudentId);
        }
        protected override async Task<StudentItemDto> MapEntityToEntityDto(Student entity)
        {
            var studentDto = new StudentItemDto();
            if (entity != null)
            {
                studentDto.Id = entity.Id;
                studentDto.UserId = entity.UserId;
                var user = await _userManager.FindByIdAsync(entity.UserId.ToString());
                if (user != null)
                {
                    studentDto.FirstName = user.FirstName;
                    studentDto.LastName = user.LastName;
                }
                if (entity.StudyGroupId.HasValue)
                {
                    var resultGetGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(entity.StudyGroupId.Value));
                    if (resultGetGroup.IsSuccessed)
                    {
                        studentDto.StudyGroup = resultGetGroup.Value;
                    }
                }
            }
            return studentDto;
        }
        private Result ErrorNotFoundStudentWithUserId(long userId)
        {
            return ErrorByIdFormat(userId, "Студента с UserId - {0} не существует");
        }
        private Result ErrorNotFoundStudentWithId(long studentId)
        {
            return ErrorByIdFormat(studentId, "Студента с Id - {0} не существует");
        }

        
    }
}
