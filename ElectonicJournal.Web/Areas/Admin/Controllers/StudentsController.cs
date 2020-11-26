using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Admin.Models.Students;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectronicJournal.Application.Dto;

namespace ElectronicJournal.Web.Areas.Admin.Controllers
{
    [Area(AreasConsts.Admin)]
    [Authorize(Roles = RolesConsts.Admin.Name)]
    public class StudentsController : ElectronicJournalControllerBase
    {
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IStudentAppService _studentService;
        public StudentsController(
            IStudyGroupAppService studyGroupAppService,
            IStudentAppService studentAppService)
        {
            _studyGroupService = studyGroupAppService;
            _studentService = studentAppService;
        }
        public async Task<IActionResult> Index()
        {
            var returnUrl = NormalizeReturnUrl(null);
            var model = new GetStudentsViewModel
            {
                ReturnUrl = returnUrl
            };
            model.Input.StudyGroupId = null;
            var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
            if (resultGetStudyGroups.IsSuccessed)
            {
                ViewBag.StudyGroups =
                   new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
            }
            var result = await _studentService.GetStudents(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetStudentsViewModel model)
        {
            var result = await _studentService.GetStudents(model.Input);
            if (result.IsSuccessed)
            {
                var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStudyGroups.IsSuccessed)
                {
                    ViewBag.StudyGroups =
                       new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
                }
                model.Value = result.Value;
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoStudent(long id)
        {
            var returnUrl = NormalizeReturnUrl(null, () => Url.Action("Index", "Students", new { Area = "Admin" }));
            var result = await _studentService.GetStudentById(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var student = result.Value;
                var model = new EditInfoStudentViewModel();
                model.Input.StudentId = student.Id;
                model.Input.StudyGroupId = student.StudyGroup?.Id;
                model.FullName = student.FullName;
                model.ReturnUrl = returnUrl;
                var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStudyGroups.IsSuccessed)
                {
                    ViewBag.StudyGroups =
                       new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
                }
                return View(model);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> EditInfoStudent(EditInfoStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _studentService.UpdateStudentInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStudyGroups.IsSuccessed)
                {
                    ViewBag.StudyGroups =
                       new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
                }
                AddResultErros(result);
                AddResultErros(resultGetStudyGroups);
            }
            return View(model);
        }

        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "Users", new { Area = AreasConsts.Admin });
        }
    }
}