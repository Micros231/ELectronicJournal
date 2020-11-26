using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores
{
    public interface IAcademicSubjectScoreAppService : IAppService
    {
        Task<Result> CreateScore(CreateScoreInput input);
        Task<Result<ScoreItemDto>> GetScore(EntityDto<long> input);
        Task<Result<ListResultDto<ScoreItemDto>>> GetScores(GetScoresInput input);

        Task<Result> UpdateScoreInfo(UpdateSocreInfoInput input);
    }
}
