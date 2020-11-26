using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Admin.Models.StudyGroups;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronicJournal.Web.Areas.Admin.Controllers
{
    [Area(AreasConsts.Admin)]
    [Authorize(Roles = RolesConsts.Admin.Name)]
    public class StudyGroupsController : ElectronicJournalControllerBase
    {
        private readonly IStudyGroupAppService _studyGroupService;
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly ITeacherAppService _teacherService;
        public StudyGroupsController(
            IStudyGroupAppService studyGroupAppService,
            IAcademicSubjectAppService academicSubjectAppService,
            ITeacherAppService teacherAppService)
        {
            _studyGroupService = studyGroupAppService;
            _academicSubjectService = academicSubjectAppService;
            _teacherService = teacherAppService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetStudyGroupsViewModel();
            model.Input.AcademicSubjectId = null;
            model.Input.TeacherId = null;
            var resultGetTeachers = await _teacherService.GetTeachers(new GetTeachersInput());
            if (resultGetTeachers.IsSuccessed)
            {
                ViewBag.Teachers =
                    new SelectList(resultGetTeachers.Value.Items, "Id", "FullName");
            }
            var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
            if (resultGetAcademicSubjects.IsSuccessed)
            {
                ViewBag.AcademicSubjects =
                    new SelectList(resultGetAcademicSubjects.Value.Items, "Id", "Name");
            }
            var result = await _studyGroupService.GetStudyGroups(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetStudyGroupsViewModel model)
        {
            var result = await _studyGroupService.GetStudyGroups(model.Input);
            if (result.IsSuccessed)
            {
                var resultGetTeachers = await _teacherService.GetTeachers(new GetTeachersInput());
                if (resultGetTeachers.IsSuccessed)
                {
                    ViewBag.Teachers =
                        new SelectList(resultGetTeachers.Value.Items, "Id", "FullName");
                }
                var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                if (resultGetAcademicSubjects.IsSuccessed)
                {
                    ViewBag.AcademicSubjects =
                        new SelectList(resultGetAcademicSubjects.Value.Items, "Id", "Name");
                }
                model.Value = result.Value;
            }
            return View(model);
        }
        public async Task<IActionResult> CreateStudyGroup(string returnUrl = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var model = new CreateStudyGroupViewModel
            {
                Input = new CreateStudyGroupInput(),
                ReturnUrl = returnUrl
            };
            var resultGetTeachers = await _teacherService.GetTeachers(new GetTeachersInput());
            if (resultGetTeachers.IsSuccessed)
            {
                var teachers = resultGetTeachers.Value;
                foreach (var teacher in teachers.Items)
                {
                    var comboboxItem = new ComboboxItemDto(teacher.Id.ToString(), teacher.FullName);
                    model.Input.TeacherComboboxes.Add(comboboxItem);
                }
            }
            var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
            if (resultGetAcademicSubjects.IsSuccessed)
            {
                var academicSubjects = resultGetAcademicSubjects.Value;
                foreach (var academicSubject in academicSubjects.Items)
                {
                    var comboboxItem = new ComboboxItemDto(academicSubject.Id.ToString(), academicSubject.Name);
                    model.Input.AcademicSubjectComboboxes.Add(comboboxItem);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudyGroup(CreateStudyGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _studyGroupService.CreateStudyGroup(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoStudyGroup(long id)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _studyGroupService.GetStudyGroup(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var studyGroup = result.Value;
                var model = new EditInfoStudyGroupViewModel();
                model.Input.StudyGroupId = studyGroup.Id;
                model.Input.Name = studyGroup.Name;
                model.ReturnUrl = returnUrl;
                var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                if (resultGetAcademicSubjects.IsSuccessed)
                {
                    var academicSubjects = resultGetAcademicSubjects.Value;
                    foreach (var academicSubject in academicSubjects.Items)
                    {
                        var comboboxItem = new ComboboxItemDto(academicSubject.Id.ToString(), academicSubject.Name);
                        var findedAcademicSubject = studyGroup.AcademicSubjects.FirstOrDefault(acadSubj => acadSubj.Id == academicSubject.Id);
                        if (findedAcademicSubject != null)
                        {
                            comboboxItem.IsSelected = true;
                        }
                        model.Input.AcademicSubjectComboboxes.Add(comboboxItem);
                    }
                }
                var resultGetTeachers = await _teacherService.GetTeachers(new GetTeachersInput());
                if (resultGetTeachers.IsSuccessed)
                {
                    var teachers = resultGetTeachers.Value;
                    foreach (var teacher in teachers.Items)
                    {
                        var comboboxItem = new ComboboxItemDto(teacher.Id.ToString(), teacher.FullName);
                        var findedTeacher = studyGroup.Teachers.FirstOrDefault(teach => teach.Id == teacher.Id);
                        if (findedTeacher != null)
                        {
                            comboboxItem.IsSelected = true;
                        }
                        model.Input.TeacherComboboxes.Add(comboboxItem);
                    }
                }
                return View(model);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> EditInfoStudyGroup(EditInfoStudyGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _studyGroupService.UpdateStudyGroupInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                var resultGetStudyGroup = await _studyGroupService.GetStudyGroup(new EntityDto<long>(model.Input.StudyGroupId));
                if (resultGetStudyGroup.IsSuccessed)
                {
                    var studyGroup = resultGetStudyGroup.Value;
                    var resultGetAcademicSubjects = await _academicSubjectService.GetAcademicSubjects(new GetAcademicSubjectsInput());
                    if (resultGetAcademicSubjects.IsSuccessed)
                    {
                        var academicSubjects = resultGetAcademicSubjects.Value;
                        foreach (var academicSubject in academicSubjects.Items)
                        {
                            var comboboxItem = new ComboboxItemDto(academicSubject.Id.ToString(), academicSubject.Name);
                            var findedAcademicSubject = studyGroup.AcademicSubjects.FirstOrDefault(acadSubj => acadSubj.Id == academicSubject.Id);
                            if (findedAcademicSubject != null)
                            {
                                comboboxItem.IsSelected = true;
                            }
                            model.Input.AcademicSubjectComboboxes.Add(comboboxItem);
                        }
                    }
                    var resultGetTeachers = await _teacherService.GetTeachers(new GetTeachersInput());
                    if (resultGetTeachers.IsSuccessed)
                    {
                        var teachers = resultGetTeachers.Value;
                        foreach (var teacher in teachers.Items)
                        {
                            var comboboxItem = new ComboboxItemDto(teacher.Id.ToString(), teacher.FullName);
                            var findedTeacher = studyGroup.Teachers.FirstOrDefault(teach => teach.Id == teacher.Id);
                            if (findedTeacher != null)
                            {
                                comboboxItem.IsSelected = true;
                            }
                            model.Input.TeacherComboboxes.Add(comboboxItem);
                        }
                    }
                }
                AddResultErros(result);
                AddResultErros(resultGetStudyGroup);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteStudyGroup(long id)
        {
            var result = await _studyGroupService.DeleteStudyGroup(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            throw new Exception(result.Errors.First().Message);
        }

        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "StudyGroups", new { Area = AreasConsts.Admin });
        }
    }
}