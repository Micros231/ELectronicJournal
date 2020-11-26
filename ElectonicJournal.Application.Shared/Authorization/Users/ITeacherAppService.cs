using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Authorization.Users
{
    public interface ITeacherAppService : IAppService
    {
        Task<Result> CreateTeacher(CreateTeacherInput input);
        Task<Result<ListResultDto<TeacherItemDto>>> GetTeachers(GetTeachersInput input);
        Task<Result<TeacherItemDto>> GetTeacherById(EntityDto<long> input);
        Task<Result<TeacherItemDto>> GetTeacherByUserId(EntityDto<long> input);
        Task<Result> DeleteTeacherById(EntityDto<long> input);
        Task<Result> DeleteTeacherByUserId(EntityDto<long> input);
        Task<Result> UpdateTeacherInfo(UpdateTeacherInfoInput input);
    }
}
