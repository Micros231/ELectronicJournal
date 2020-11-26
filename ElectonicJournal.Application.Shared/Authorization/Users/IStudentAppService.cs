using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Authorization.Users
{
    public interface IStudentAppService : IAppService
    {
        Task<Result> CreateStudent(CreateStudentInput input);
        Task<Result<ListResultDto<StudentItemDto>>> GetStudents(GetStudentsInput input);
        Task<Result<StudentItemDto>> GetStudentById(EntityDto<long> input);
        Task<Result<StudentItemDto>> GetStudentByUserId(EntityDto<long> input);
        Task<Result> DeleteStudentById(EntityDto<long> input);
        Task<Result> DeleteStudentByUserId(EntityDto<long> input);
        Task<Result> UpdateStudentInfo(UpdateStudentInfoInput input);
    }
}
