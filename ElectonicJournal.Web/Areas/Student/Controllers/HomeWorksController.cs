using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.HomeWorks;
using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Areas.Student.Models.HomeWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicJournal.Web.Areas.Student.Controllers
{
    [Area(AreasConsts.Student)]
    [Authorize(Roles = RolesConsts.Student.Name)]
    public class HomeWorksController : Controller
    {
        private readonly IHomeWorkAppService _homeWorkService;
        private readonly IStudentAppService _studentService;
        private readonly IAcademicSubjectAppService _subjectService;
        private readonly IUserAppService _userService;
        public HomeWorksController(
            IHomeWorkAppService homeWorkService,
            IStudentAppService studentService,
            IAcademicSubjectAppService subjectService,
            IUserAppService userService)
        {
            _homeWorkService = homeWorkService;
            _studentService = studentService;
            _subjectService = subjectService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetHomeWorksViewModel();
            var student = await GetStudent();
            if (student.StudyGroup != null)
            {
                model.Input.StudyGroupId = student.StudyGroup.Id;
                var dateTime = DateTime.Now;
                model.Input.DateString = $"01." +
                    $"{(dateTime.Month < 10 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString())}." +
                    $"{dateTime.Year}";
                var result = await _homeWorkService.GetHomeWorks(model.Input);
                if (result.IsSuccessed)
                {
                    model.Value = result.Value;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetHomeWorksViewModel model)
        {
            var result = await _homeWorkService.GetHomeWorks(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetHomeWork(long homeWorkId)
        {
            var result = await _homeWorkService.GetHomeWorks(new GetHomeWorksInput());
            if (result.IsSuccessed)
            {
                var homeWorks = result.Value;
                var firstHomeWork = homeWorks.Items.FirstOrDefault(homework => homework.Id == homeWorkId);
                if (firstHomeWork != null)
                {
                    if (firstHomeWork.HomeWorkData != null)
                    {
                        string fileName = $"{firstHomeWork.Id}_" +
                            $"{firstHomeWork.StudyGroup.Name}_" +
                            $"{firstHomeWork.AcademicSubject.Name}_" +
                            $"{firstHomeWork.EndDate.ToShortDateString()}.rtf";
                        return File(firstHomeWork.HomeWorkData, "application/rtf", fileName);
                    }
                }
            }
            return RedirectToAction("Index");
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