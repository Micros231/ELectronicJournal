using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.HomeWorks;
using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Extenstions;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Areas.Teacher.Models.HomeWorks;
using ElectronicJournal.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicJournal.Web.Areas.Teacher.Controllers
{
    [Area(AreasConsts.Teacher)]
    [Authorize(Roles = RolesConsts.Teacher.Name)]
    public class HomeWorksController : ElectronicJournalControllerBase
    {
        private readonly IHomeWorkAppService _homeWorkService;
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly ITeacherAppService _teacherService;
        private readonly IUserAppService _userService;
        public HomeWorksController(
            IHomeWorkAppService homeWorkService,
            IStudyGroupAppService studyGroupService,
            ITeacherAppService teacherService,
            IUserAppService userService)
        {
            _homeWorkService = homeWorkService;
            _studyGroupService = studyGroupService;
            _teacherService = teacherService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetHomeWorksViewModel();
            var teacher = await GetTeacher();
            if (!teacher.StudyGroups.IsNullOrEmpty())
            {
                ViewBag.StudyGroups =
                    new SelectList(teacher.StudyGroups, "Id", "Name");
                model.Input.StudyGroupId = teacher.StudyGroups.First().Id;
            }
            if (teacher.AcademicSubject != null)
            {
                model.Input.AcademicSubjectId = teacher.AcademicSubject.Id;
            }
            var dateTime = DateTime.Now;
            model.Input.DateString = $"01." +
                $"{(dateTime.Month < 10 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString())}." +
                $"{dateTime.Year}";

            var result = await _homeWorkService.GetHomeWorks(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetHomeWorksViewModel model)
        {
            var result = await _homeWorkService.GetHomeWorks(model.Input);
            if (result.IsSuccessed)
            {
                var teacher = await GetTeacher();
                if (!teacher.StudyGroups.IsNullOrEmpty())
                {
                    ViewBag.StudyGroups =
                        new SelectList(teacher.StudyGroups, "Id", "Name");
                }
                model.Value = result.Value;
            }
            return View(model);
        }
        public async Task<IActionResult> CreateHomeWork(GetCreateHomeWorkViewModel model, string returnUrl = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var createModel = new CreateHomeWorkViewModel();
            if (model.StudyGroupId.HasValue)
            {
                var resultGetGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(model.StudyGroupId.Value));
                if (resultGetGroup.IsSuccessed)
                {
                    createModel.StudyGroup = resultGetGroup.Value;
                    createModel.Input.StudyGroupId = model.StudyGroupId.Value;
                }
            }
            if (model.SubjectId.HasValue)
            {
                var teacher = await GetTeacher();
                if (teacher.AcademicSubject != null)
                {
                    createModel.Subject = teacher.AcademicSubject;
                    createModel.Input.AcademicSubjectId = model.SubjectId.Value;

                }
            }
            createModel.ReturnUrl = returnUrl;
            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHomeWork(CreateHomeWorkViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.HomeWorkFile != null)
                {
                    byte[] data = null;
                    using (var binaryReader = new BinaryReader(model.HomeWorkFile.OpenReadStream()))
                    {
                        data = binaryReader.ReadBytes((int)model.HomeWorkFile.Length);
                    }
                    model.Input.HomeWorkData = data;
                }
                var result = await _homeWorkService.CreateHomeWork(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoHomeWork(long homeWorkId)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _homeWorkService.GetHomeWorks(new GetHomeWorksInput());
            if (result.IsSuccessed)
            {
                var homeWorks = result.Value;
                var homeWork = homeWorks.Items.FirstOrDefault(homeWork => homeWork.Id == homeWorkId);
                if (homeWork != null)
                {
                    var model = new EditInfoHomeWorkViewModel();
                    model.ReturnUrl = returnUrl;
                    if (homeWork.AcademicSubject != null)
                    {
                        model.Subject = homeWork.AcademicSubject;
                    }
                    if (homeWork.StudyGroup != null)
                    {
                        model.StudyGroup = homeWork.StudyGroup;
                    }
                    model.Input.Description = homeWork.Description;
                    model.Input.HomeWorkId = homeWork.Id;
                    return View(model);
                }
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> EditInfoHomeWork(EditInfoHomeWorkViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.HomeWorkFile != null)
                {
                    byte[] data = null;
                    using (var binaryReader = new BinaryReader(model.HomeWorkFile.OpenReadStream()))
                    {
                        data = binaryReader.ReadBytes((int)model.HomeWorkFile.Length);
                    }
                    model.Input.HomeWorkData = data;
                }
                var result = await _homeWorkService.UpdateHomeWork(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "HomeWorks", new { Area = AreasConsts.Teacher });
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
    }
}