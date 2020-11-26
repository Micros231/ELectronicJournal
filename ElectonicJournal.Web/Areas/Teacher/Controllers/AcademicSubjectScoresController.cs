using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjectScores;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Extenstions;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Areas.Teacher.Models;
using ElectronicJournal.Web.Areas.Teacher.Models.AcademicSubjectScores;
using ElectronicJournal.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto;

namespace ElectronicJournal.Web.Areas.Teacher.Controllers
{
    [Area(AreasConsts.Teacher)]
    [Authorize(Roles = RolesConsts.Teacher.Name)]
    public class AcademicSubjectScoresController : ElectronicJournalControllerBase
    {
        private readonly IAcademicSubjectScoreAppService _scoreService;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IStudentAppService _studentService;
        private readonly ITeacherAppService _teacherService;
        private readonly IUserAppService _userService;

        public AcademicSubjectScoresController(
            IAcademicSubjectScoreAppService scoresService,
            IStudyGroupAppService studyGroupService,
            IStudentAppService studentService,
            ITeacherAppService teacherService,
            IUserAppService userService)
        {
            _scoreService = scoresService;
            _studentService = studentService;
            _studyGroupService = studyGroupService;
            _teacherService = teacherService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new GetScoresViewModel();
            TeacherItemDto teacher = await GetTeacher();
            model.Input.StudentId = null;
            model.Input.TeacherId = teacher.Id;
            if (!teacher.StudyGroups.IsNullOrEmpty())
            {
                ViewBag.StudyGroups =
                    new SelectList(teacher.StudyGroups, "Id", "Name");
                model.Input.StudyGroupId = teacher.StudyGroups.First().Id;
            }
            if (model.Input.StudyGroupId.HasValue)
            {
                var resultGetStudents = 
                    await _studentService.GetStudents(
                        new GetStudentsInput { StudyGroupId = model.Input.StudyGroupId });
                if (resultGetStudents.IsSuccessed)
                {
                    model.Students = resultGetStudents.Value.Items.ToList();
                }
            }
            var dateTime = DateTime.Now;
            model.Input.DateString = $"01." +
                $"{(dateTime.Month < 10 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString())}." +
                $"{dateTime.Year}";
            var daysInMonth = DateTime.DaysInMonth(model.Input.Date.Value.Year, model.Input.Date.Value.Month);
            var days = new List<DayItemDto>();
            for (int i = 0; i < daysInMonth; i++)
            {
                var dayItem = new DayItemDto
                {
                    DayInMonth = i + 1,
                    DateTime =
                    new DateTime(model.Input.Date.Value.Year, model.Input.Date.Value.Month, i + 1)
                };
                dayItem.DateString =
                        $"{(dayItem.DateTime.Day < 10 ? "0" + dayItem.DateTime.Day.ToString() : dayItem.DateTime.Day.ToString())}." +
                        $"{(dayItem.DateTime.Month < 10 ? "0" + dayItem.DateTime.Month.ToString() : dayItem.DateTime.Month.ToString())}." +
                        $"{dayItem.DateTime.Year}";
                days.Add(dayItem);
            }
            model.Days = days;
            if (teacher.AcademicSubject != null)
            {
                model.AcademicSubject = teacher.AcademicSubject;
            }
            var result = await _scoreService.GetScores(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(GetScoresViewModel model)
        {

            var result = await _scoreService.GetScores(model.Input);
            if (result.IsSuccessed)
            {
                TeacherItemDto teacher = await GetTeacher();
                model.Input.TeacherId = teacher.Id;
                if (!teacher.StudyGroups.IsNullOrEmpty())
                {
                    ViewBag.StudyGroups =
                        new SelectList(teacher.StudyGroups, "Id", "Name");
                }
                if (model.Input.StudyGroupId.HasValue)
                {
                    var resultGetStudents =
                        await _studentService.GetStudents(
                            new GetStudentsInput { StudyGroupId = model.Input.StudyGroupId });
                    if (resultGetStudents.IsSuccessed)
                    {
                        model.Students = resultGetStudents.Value.Items.ToList();
                    }
                }
                var daysInMonth = DateTime.DaysInMonth(model.Input.Date.Value.Year, model.Input.Date.Value.Month);
                var days = new List<DayItemDto>();
                for (int i = 0; i < daysInMonth; i++)
                {
                    var dayItem = new DayItemDto
                    {
                        DayInMonth = i + 1,
                        DateTime =
                        new DateTime(model.Input.Date.Value.Year, model.Input.Date.Value.Month, i + 1),
                    };
                    dayItem.DateString =
                        $"{(dayItem.DateTime.Day < 10 ? "0" + dayItem.DateTime.Day.ToString() : dayItem.DateTime.Day.ToString())}." +
                        $"{(dayItem.DateTime.Month < 10 ? "0" + dayItem.DateTime.Month.ToString() : dayItem.DateTime.Month.ToString())}." +
                        $"{dayItem.DateTime.Year}";
                    days.Add(dayItem);
                }
                model.Days = days;
                if (teacher.AcademicSubject != null)
                {
                    model.AcademicSubject = teacher.AcademicSubject;
                }
                model.Value = result.Value;
            }
            return View(model);
        }
        public async Task<IActionResult> CreateScore(GetCreateScoreViewModel model, string returnUrl = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var createModel = new CreateScoreViewModel
            {
                ReturnUrl = returnUrl,
                Input = new CreateScoreInput()
            };
            var resultGetTeacher = await _teacherService.GetTeacherById(new EntityDto<long>(model.TeacherId));
            if (resultGetTeacher.IsSuccessed)
            {
                var teacher = resultGetTeacher.Value;
                createModel.Input.TeacherId = model.TeacherId;
                if (teacher.AcademicSubject != null)
                {
                    if (teacher.AcademicSubject.Id == model.SubjectId)
                    {
                        createModel.Input.AcademicSubjectId = model.SubjectId;
                        createModel.AcademicSubject = teacher.AcademicSubject;
                    }
                }
                
            }
            
            createModel.Input.StudyGroupId = model.StudyGroupId;
            var resultGetStudent = await _studentService.GetStudentById(new EntityDto<long>(model.StudentId));
            if (resultGetStudent.IsSuccessed)
            {
                createModel.Input.StudentId = model.StudentId;
                createModel.StudentName = resultGetStudent.Value.FullName;
            }
            createModel.Input.DateString = model.DateString;
            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateScore(CreateScoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _scoreService.CreateScore(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoScore(long scoreId)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _scoreService.GetScore(new EntityDto<long>(scoreId));
            if (result.IsSuccessed)
            {
                var score = result.Value;
                var model = new EditInfoScoreViewModel();
                model.ReturnUrl = returnUrl;
                model.Input.ScoreId = score.Id;
                model.Input.Score = score.Score;
                if (score.Student != null)
                {
                    model.StudentName = score.Student.FullName;
                }
                if (score.AcademicSubject != null)
                {
                    model.AcademicSubject = score.AcademicSubject;
                }
                return View(model);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> EditInfoScore(EditInfoScoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _scoreService.UpdateScoreInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        private async Task<TeacherItemDto> GetTeacher()
        {
            var teacher = new TeacherItemDto();
            var resultGetUser = await _userService.GetUserByClaims(User);
            if (resultGetUser.IsSuccessed)
            {
                var user = resultGetUser.Value;
                var resultGetTeacher = await _teacherService.GetTeacherByUserId(new EntityDto<long>(user.Id));
                if (resultGetTeacher.IsSuccessed)
                {
                    teacher = resultGetTeacher.Value;
                }
            }
            return teacher;
        }

        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "AcademicSubjectScores", new { Area = AreasConsts.Teacher });
        }
    }
}