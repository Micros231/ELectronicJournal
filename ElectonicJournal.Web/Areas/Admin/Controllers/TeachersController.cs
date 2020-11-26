using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Web.Areas.Admin.Models.Teachers;
using Microsoft.AspNetCore.Mvc;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Web.Areas.Startup;
using Microsoft.AspNetCore.Authorization;
using ElectronicJournal.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Web.Controllers;

namespace ElectronicJournal.Web.Areas.Admin.Controllers
{
    [Area(AreasConsts.Admin)]
    [Authorize(Roles = RolesConsts.Admin.Name)]
    public class TeachersController : ElectronicJournalControllerBase
    {
        private readonly ITeacherAppService _teacherService;
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly IStudyGroupAppService _studyGroupService;
        public TeachersController(
            ITeacherAppService teacherService,
            IAcademicSubjectAppService academicSubjectAppService,
            IStudyGroupAppService studyGroupAppService)
        {
            _teacherService = teacherService;
            _academicSubjectService = academicSubjectAppService;
            _studyGroupService = studyGroupAppService;
        }

        public async Task<IActionResult> Index()
        {
            var returnUrl = NormalizeReturnUrl(null);
            var model = new GetTeachersViewModel
            {
                Input = new GetTeachersInput(),
                ReturnUrl = returnUrl
            };
            model.Input.AcademicSubjectId = null;
            model.Input.StudyGroupId = null;
            var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
            if (resultGetAcademicSubjects.IsSuccessed)
            {
                ViewBag.AcademicSubjects =
                    new SelectList(resultGetAcademicSubjects.Value.Items, "Id", "Name");
            }
            var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
            if (resultGetStudyGroups.IsSuccessed)
            {
                ViewBag.StudyGroups =
                    new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
            }
            var result = await _teacherService.GetTeachers(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetTeachersViewModel model)
        {
            var result = await _teacherService.GetTeachers(model.Input);
            if (result.IsSuccessed)
            {
                var resultGetAcadSubjs = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                if (resultGetAcadSubjs.IsSuccessed)
                {
                    ViewBag.AcademicSubjects =
                        new SelectList(resultGetAcadSubjs.Value.Items, "Id", "Name");
                }
                var resultGetStdGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStdGroups.IsSuccessed)
                {
                    ViewBag.StudyGroups =
                        new SelectList(resultGetStdGroups.Value.Items, "Id", "Name");
                }
                model.Value = result.Value;
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoTeacher(long id)
        {
            
            var returnUrl = NormalizeReturnUrl(null, () => Url.Action("Index", "Teachers", new { Area = "Admin" }));
            var result = await _teacherService.GetTeacherById(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var teacher = result.Value;
                var model = new EditInfoTeacherViewModel();
                model.Input.TeacherId = teacher.Id;
                model.Input.AcademicSubjectId = teacher.AcademicSubject?.Id;
                model.FullName = teacher.FullName;
                model.ReturnUrl = returnUrl;
                var resultGetAcadSubjs = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                if (resultGetAcadSubjs.IsSuccessed)
                {
                    ViewBag.AcademicSubjects =
                        new SelectList(resultGetAcadSubjs.Value.Items, "Id", "Name");
                }
                var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStudyGroups.IsSuccessed)
                {
                    var studyGroups = resultGetStudyGroups.Value;
                    foreach (var studyGroup in studyGroups.Items)
                    {
                        var comboboxItem = new ComboboxItemDto(studyGroup.Id.ToString(), studyGroup.Name);
                        var findedStudyGroup = teacher.StudyGroups.FirstOrDefault(studyGroup => studyGroup.Id == studyGroup.Id);
                        if (findedStudyGroup != null)
                        {
                            comboboxItem.IsSelected = true;
                        }
                        model.Input.StudyGroupComboboxes.Add(comboboxItem);
                    }
                }
                return View(model);
            }
            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> EditInfoTeacher(EditInfoTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _teacherService.UpdateTeacherInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                var resultGetTeacher = await _teacherService.GetTeacherById(new EntityDto<long>(model.Input.TeacherId));
                if (resultGetTeacher.IsSuccessed)
                {
                    var teacher = resultGetTeacher.Value;
                    var resultGetAcadSubjs = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                    if (resultGetAcadSubjs.IsSuccessed)
                    {
                        ViewBag.AcademicSubjects =
                            new SelectList(resultGetAcadSubjs.Value.Items, "Id", "Name");
                    }
                    var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                    if (resultGetStudyGroups.IsSuccessed)
                    {
                        var studyGroups = resultGetStudyGroups.Value;
                        foreach (var studyGroup in studyGroups.Items)
                        {
                            var comboboxItem = new ComboboxItemDto(studyGroup.Id.ToString(), studyGroup.Name);
                            var findedStudyGroup = teacher.StudyGroups.FirstOrDefault(studyGroup => studyGroup.Id == studyGroup.Id);
                            if (findedStudyGroup != null)
                            {
                                comboboxItem.IsSelected = true;
                            }
                            model.Input.StudyGroupComboboxes.Add(comboboxItem);
                        }
                    }
                }
                AddResultErros(result);
                AddResultErros(resultGetTeacher);
            }
            return View(model);
        }

        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "Users", new { Area = AreasConsts.Admin });
        }
    }
}