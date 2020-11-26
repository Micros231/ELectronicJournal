using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjectScores;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Areas.Student.Models.AcademicSubjectScores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Areas.Student.Controllers
{
    [Area(AreasConsts.Student)]
    [Authorize(Roles = RolesConsts.Student.Name)]
    public class AcademicSubjectScoresController : Controller
    {
        private readonly IAcademicSubjectScoreAppService _scoreService;
        private readonly IStudentAppService _studentService;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IUserAppService _userService;

        public AcademicSubjectScoresController(
            IAcademicSubjectScoreAppService scoresService,
            IStudentAppService studentService,
            IStudyGroupAppService studyGroupService,
            IUserAppService userService)
        {
            _scoreService = scoresService;
            _studentService = studentService;
            _userService = userService;
            _studyGroupService = studyGroupService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new GetScoresViewModel();
            var student = await GetStudent();
            model.Input.StudentId = student.Id;
            model.Input.TeacherId = null;
            model.Input.StudyGroupId = null;
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
            if (student.StudyGroup != null)
            {
                var resultGetStudyGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(student.StudyGroup.Id));
                if (resultGetStudyGroup.IsSuccessed)
                {
                    var studyGroup = resultGetStudyGroup.Value;
                    model.Subjects = studyGroup.AcademicSubjects;
                }
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
                var student = await GetStudent();
                model.Input.StudentId = student.Id;
                model.Input.TeacherId = null;
                model.Input.StudyGroupId = null;
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
                if (student.StudyGroup != null)
                {
                    var resultGetStudyGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(student.StudyGroup.Id));
                    if (resultGetStudyGroup.IsSuccessed)
                    {
                        var studyGroup = resultGetStudyGroup.Value;
                        model.Subjects = studyGroup.AcademicSubjects;
                    }
                }
                model.Value = result.Value;
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