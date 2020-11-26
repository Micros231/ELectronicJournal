using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjectScores;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Areas.Student.Models.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Areas.Student.Controllers
{
    [Area(AreasConsts.Student)]
    [Authorize(Roles = RolesConsts.Student.Name)]
    public class RatingController : Controller
    {
        private readonly IStudentAppService _studentService;
        private readonly IAcademicSubjectScoreAppService _scoreService;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IUserAppService _userService;

        public RatingController(
            IStudentAppService studentAppService,
            IAcademicSubjectScoreAppService scoreService,
            IUserAppService userService,
            IStudyGroupAppService studyGroupService)
        {
            _studentService = studentAppService;
            _scoreService = scoreService;
            _userService = userService;
            _studyGroupService = studyGroupService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new GetRatingViewModel();
            var student = await GetStudent();
            if (student.StudyGroup != null)
            {
                var resultGetStudents = 
                    await _studentService.GetStudents(new GetStudentsInput { StudyGroupId = student.StudyGroup.Id });
                if (resultGetStudents.IsSuccessed)
                {
                    foreach (var stud in resultGetStudents.Value.Items)
                    {
                        var studentRating = new StudentRatingViewModel();
                        studentRating.Student = stud;
                        var resultGetScores = await _scoreService.GetScores(new GetScoresInput() { StudentId = stud.Id });
                        if (resultGetScores.IsSuccessed)
                        {
                            var scores = resultGetScores.Value.Items;
                            float ratingScore = 0;
                            foreach (var score in scores)
                            {
                                ratingScore += score.Score;
                            }
                            studentRating.Score = ratingScore / scores.Count;
                            model.StudentRatings.Add(studentRating);
                        }
                    }
                }
                model.StudentRatings = model.StudentRatings.OrderByDescending(rating => rating.Score).ToList();
            }
            return View(model);
        }
        private async Task<StudentItemDto> GetStudent()
        {
            var student = new StudentItemDto();
            var resultGetUser = await _userService.GetUserByClaims(User);
            if (resultGetUser.IsSuccessed)
            {
                var user = resultGetUser.Value;
                var resultGetStudent = await _studentService.GetStudentByUserId(new EntityDto<long>(user.Id));
                if (resultGetStudent.IsSuccessed)
                {
                    student = resultGetStudent.Value;
                }
            }
            return student;
        }
    }
}