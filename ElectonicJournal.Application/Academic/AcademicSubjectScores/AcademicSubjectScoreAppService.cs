using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.AppService;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
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

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores
{
    public class AcademicSubjectScoreAppService : AppServiceBase<AcademicSubjectScore, ScoreItemDto, long>, IAcademicSubjectScoreAppService
    {
        private readonly IRepository<AcademicSubjectScore, long> _academicSubjectScoreRepository;
        private readonly ITeacherAppService _teacherService;
        private readonly IStudentAppService _studentService;
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly IStudyGroupAppService _studyGroupService;
        public AcademicSubjectScoreAppService(
            IRepository<AcademicSubjectScore, long> academicSubjectScoreRepository,
            ITeacherAppService teacherService,
            IStudentAppService studentService,
            IAcademicSubjectAppService academicSubjectService,
            IStudyGroupAppService studyGroupAppService)
        {
            _academicSubjectScoreRepository = academicSubjectScoreRepository;
            _teacherService = teacherService;
            _studentService = studentService;
            _academicSubjectService = academicSubjectService;
            _studyGroupService = studyGroupAppService;
        }
        public async Task<Result> CreateScore(CreateScoreInput input)
        {
            var errorList = new List<ErrorResult>();
            if (input.Date.HasValue)
            {
                bool check = await CheckStudentContinsInStudyGroup(input.StudentId, input.StudyGroupId);
                if (check)
                {
                    check = await CheckTeacherContainsStudyGroup(input.TeacherId, input.StudyGroupId);
                    if (check)
                    {
                        check = await CheckTeacherContainsAcademicSubject(input.TeacherId, input.AcademicSubjectId);
                        if (check)
                        {
                            check = await CheckStudyGroupContainsAcademicSubject(input.StudyGroupId, input.AcademicSubjectId);
                            if (check)
                            {
                                var score = new AcademicSubjectScore(
                                input.StudentId,
                                input.TeacherId,
                                input.AcademicSubjectId,
                                input.Score,
                                input.Date.Value);
                                await _academicSubjectScoreRepository.InsertAsync(score);
                                return Result.Success();
                            }
                            errorList.Add(new ErrorResult($"Группа {input.StudyGroupId} не обучается предмету {input.AcademicSubjectId}"));
                        }
                        errorList.Add(new ErrorResult($"Учитель {input.TeacherId} не обучает предмету {input.AcademicSubjectId}"));
                    }
                    errorList.Add(new ErrorResult($"Учитель {input.TeacherId} не обучает группу {input.StudyGroupId}"));
                }
                errorList.Add(new ErrorResult($"Студент {input.StudentId} не принадлежит группе {input.StudyGroupId}"));
                
            }
            errorList.Add(new ErrorResult($"Дата имеет не верный формат"));
            return Result.Failed(errorList);
        }
        public async Task<Result<ScoreItemDto>> GetScore(EntityDto<long> input)
        {
            var score = await _academicSubjectScoreRepository.GetAsync(input.Id);
            if (score != null)
            {
                var scoreDto = await MapEntityToEntityDto(score);
                return Result<ScoreItemDto>.Success(scoreDto);
            }
            return ErrorNotFoundScoreWithId(input.Id);
        }
        public async Task<Result<ListResultDto<ScoreItemDto>>> GetScores(GetScoresInput input)
        {
            var query = _academicSubjectScoreRepository.GetAll();

            var scores = await query.ToListAsync();
            if (input.StudentId.HasValue)
            {
                scores = scores.Where(score => score.StudentId == input.StudentId.Value).ToList();
            }
            if (input.TeacherId.HasValue)
            {
                scores = scores.Where(score => score.TeacherId == input.TeacherId.Value).ToList();
            }
            if (input.Date.HasValue)
            {
                scores = scores.Where(score =>
                score.Date.Month == input.Date.Value.Month && score.Date.Year == input.Date.Value.Year).ToList();
            }
            if (input.StudyGroupId.HasValue)
            {
                var resultGetStudents = await _studentService.GetStudents(new GetStudentsInput { StudyGroupId = input.StudyGroupId });
                if (resultGetStudents.IsSuccessed)
                {
                    var students = resultGetStudents.Value;
                    scores = (from score in scores
                             from student in students.Items
                             where score.StudentId == student.Id
                             select score).ToList();
                }
            }
            var scoreDtos = new List<ScoreItemDto>();
            foreach (var score in scores)
            {
                var scoreDto = await MapEntityToEntityDto(score);
                scoreDtos.Add(scoreDto);
            }
            return Result<ListResultDto<ScoreItemDto>>.Success(new ListResultDto<ScoreItemDto>(scoreDtos));
        }
        public async Task<Result> UpdateScoreInfo(UpdateSocreInfoInput input)
        {
            var score = await _academicSubjectScoreRepository.GetAsync(input.ScoreId);
            if (score != null)
            {
                score.Score = input.Score;
                await _academicSubjectScoreRepository.UpdateAsync(score);
                return Result.Success();
            }
            return ErrorNotFoundScoreWithId(input.ScoreId);
        }
        protected override async Task<ScoreItemDto> MapEntityToEntityDto(AcademicSubjectScore entity)
        {
            var scoreDto = new ScoreItemDto();
            if (entity != null)
            {
                scoreDto.Id = entity.Id;
                scoreDto.Score = entity.Score;
                scoreDto.Date = entity.Date;
                var resultGetStudent = 
                    await _studentService.GetStudentById(new EntityDto<long>(entity.StudentId));
                if (resultGetStudent.IsSuccessed)
                {
                    scoreDto.Student = resultGetStudent.Value;
                }
                var resultGetTeacher = 
                    await _teacherService.GetTeacherById(new EntityDto<long>(entity.TeacherId));
                if (resultGetTeacher.IsSuccessed)
                {
                    scoreDto.Teacher = resultGetTeacher.Value;
                }
                var resultGetAcademisSubject = 
                    await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(entity.AcademicSubjectId));
                if (resultGetAcademisSubject.IsSuccessed)
                {
                    scoreDto.AcademicSubject = resultGetAcademisSubject.Value;
                }
            }
            return scoreDto;
        }
        private Result ErrorNotFoundScoreWithId(long scoreId)
        {
            return ErrorByIdFormat(scoreId, "Оценки с Id - {0} не существует");
        }
        private async Task<bool> CheckStudyGroupContainsAcademicSubject(long studyGroupId, long academicSubjectId)
        {
            StudyGroupItemDto studyGroup = new StudyGroupItemDto();
            var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(studyGroupId));
            if (result.IsSuccessed)
            {
                studyGroup = result.Value;
            }
            if (studyGroup.AcademicSubjects.IsNullOrEmpty())
            {
                return false;
            }
            var firstFindedAcadSubj = studyGroup.AcademicSubjects.FirstOrDefault(first => first.Id == academicSubjectId);
            return firstFindedAcadSubj != null;
        }

        private async Task<bool> CheckTeacherContainsAcademicSubject(long teacherId, long academicSubjectId)
        {
            TeacherItemDto teacher = new TeacherItemDto();
            var result = await _teacherService.GetTeacherById(new EntityDto<long>(teacherId));
            if (result.IsSuccessed)
            {
                teacher = result.Value;
            }
            if (teacher.AcademicSubject == null)
            {
                return false;
            }
            return teacher.AcademicSubject.Id == academicSubjectId;
        }

        private async Task<bool> CheckTeacherContainsStudyGroup(long teacherId, long studyGroupId)
        {
            TeacherItemDto teacher = new TeacherItemDto();
            var result = await _teacherService.GetTeacherById(new EntityDto<long>(teacherId));
            if (result.IsSuccessed)
            {
                teacher = result.Value;
            }
            if (teacher.StudyGroups.IsNullOrEmpty())
            {
                return false;
            }
            var firstFindedGroup = teacher.StudyGroups.FirstOrDefault(group => group.Id == studyGroupId);
            return firstFindedGroup != null;
        }

        private async Task<bool> CheckStudentContinsInStudyGroup(long studentId, long studyGroupId)
        {
            StudentItemDto student = new StudentItemDto();
            var resultGetStudent = await _studentService.GetStudentById(new EntityDto<long>(studentId));
            if (resultGetStudent.IsSuccessed)
            {
                student = resultGetStudent.Value;
            }
            if (student.StudyGroup == null)
            {
                return false;
            }
            return student.StudyGroup.Id == studyGroupId;
        }

        
    }
}
