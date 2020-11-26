using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Academic.AcademicSubjects
{
    public interface IAcademicSubjectAppService : IAppService
    {
        Task<Result> CreateAcademicSubject(CreateAcademicSubjectInput input);
        Task<Result> DeleteAcademicSubject(EntityDto<long> input);
        Task<Result<AcademicSubjectItemDto>> GetAcademicSubject(EntityDto<long> input);
        Task<Result<ListResultDto<AcademicSubjectItemDto>>> GetAcademicSubjects(GetAcademicSubjectsInput input);
        Task<Result> UpdateAcademicSubjectInfo(UpdateAcademicSubjectInput input);
    }
}
