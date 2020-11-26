using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Academic.StudyGroups
{
    public interface IStudyGroupAppService : IAppService
    {
        Task<Result> CreateStudyGroup(CreateStudyGroupInput input);
        Task<Result> DeleteStudyGroup(EntityDto<long> input);
        Task<Result<StudyGroupItemDto>> GetStudyGroup(EntityDto<long> input);
        Task<Result<ListResultDto<StudyGroupItemDto>>> GetStudyGroups(GetStudyGroupsInput input);
        Task<Result> UpdateStudyGroupInfo(UpdateStudyGroupInfo input);

    }
}
