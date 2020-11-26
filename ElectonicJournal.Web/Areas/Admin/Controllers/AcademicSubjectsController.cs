using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Dto;
using ElectronicJournal.Consts;
using ElectronicJournal.Web.Areas.Admin.Models.AcademicSubjects;
using ElectronicJournal.Web.Areas.Startup;
using ElectronicJournal.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;

namespace ElectronicJournal.Web.Areas.Admin.Controllers
{
    [Area(AreasConsts.Admin)]
    [Authorize(Roles = RolesConsts.Admin.Name)]
    public class AcademicSubjectsController : ElectronicJournalControllerBase
    {
        private readonly IAcademicSubjectAppService _academicSubjectService;
        private readonly IStudyGroupAppService _studyGroupService;

        public AcademicSubjectsController(
            IAcademicSubjectAppService academicSubjectAppService,
            IStudyGroupAppService studyGroupAppService)
        {
            _academicSubjectService = academicSubjectAppService;
            _studyGroupService = studyGroupAppService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetAcademicSubjectsViewModel();
            model.Input.StudyGroupId = null;
            var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
            if (resultGetStudyGroups.IsSuccessed)
            {
                ViewBag.StudyGroups =
                    new SelectList(resultGetStudyGroups.Value.Items, "Id", "Name");
            }
            var result = await _academicSubjectService.GetAcademicSubjects(model.Input);
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(GetAcademicSubjectsViewModel model)
        {
            var result = await _academicSubjectService.GetAcademicSubjects(model.Input);
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
        public async Task<IActionResult> CreateAcademicSubject(string returnUrl = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);
            var model = new CreateAcademicSubjectViewModel
            {
                Input = new CreateAcademicSubjectInput(),
                ReturnUrl = returnUrl
            };
            var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
            if (resultGetStudyGroups.IsSuccessed)
            {
                var studyGroups = resultGetStudyGroups.Value;
                foreach (var studyGroup in studyGroups.Items)
                {
                    var comboboxItem = new ComboboxItemDto(studyGroup.Id.ToString(), studyGroup.Name);
                    model.Input.StudyGroupComboboxes.Add(comboboxItem);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAcademicSubject(CreateAcademicSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _academicSubjectService.CreateAcademicSubject(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                AddResultErros(result);
            }
            return View(model);
        }
        public async Task<IActionResult> EditInfoAcademicSubject(long id)
        {
            var returnUrl = NormalizeReturnUrl(null);
            var result = await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                var academicSubject = result.Value;
                var model = new EditInfoAcademicSubjectViewModel();
                model.Input.AcademicSubjectId = academicSubject.Id;
                model.Input.Name = academicSubject.Name;
                model.ReturnUrl = returnUrl;
                var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                if (resultGetStudyGroups.IsSuccessed)
                {
                    var studyGroups = resultGetStudyGroups.Value;
                    foreach (var studyGroup in studyGroups.Items)
                    {
                        var comboboxItem = new ComboboxItemDto(studyGroup.Id.ToString(), studyGroup.Name);
                        var findedStudyGroup = academicSubject.StudyGroups.FirstOrDefault(studyGroup => studyGroup.Id == studyGroup.Id);
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
        public async Task<IActionResult> EditInfoAcademicSubject(EditInfoAcademicSubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _academicSubjectService.UpdateAcademicSubjectInfo(model.Input);
                if (result.IsSuccessed)
                {
                    return Redirect(model.ReturnUrl);
                }
                var resultGetAcademicSubject = await _academicSubjectService.GetAcademicSubject(new EntityDto<long>(model.Input.AcademicSubjectId));
                if (resultGetAcademicSubject.IsSuccessed)
                {
                    var academicSubject = resultGetAcademicSubject.Value;
                    var resultGetStudyGroups = await _studyGroupService.GetStudyGroups(new GetStudyGroupsInput());
                    if (resultGetStudyGroups.IsSuccessed)
                    {
                        var studyGroups = resultGetStudyGroups.Value;
                        foreach (var studyGroup in studyGroups.Items)
                        {
                            var comboboxItem = new ComboboxItemDto(studyGroup.Id.ToString(), studyGroup.Name);
                            var findedStudyGroup = academicSubject.StudyGroups.FirstOrDefault(studyGroup => studyGroup.Id == studyGroup.Id);
                            if (findedStudyGroup != null)
                            {
                                comboboxItem.IsSelected = true;
                            }
                            model.Input.StudyGroupComboboxes.Add(comboboxItem);
                        }
                    }
                }
                AddResultErros(result);
                AddResultErros(resultGetAcademicSubject);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAcademicSubject(long id)
        {
            var result = await _academicSubjectService.DeleteAcademicSubject(new EntityDto<long>(id));
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            throw new Exception(result.Errors.First().Message);
        }
        protected override string GetDefaultUrl()
        {
            return Url.Action("Index", "AcademicSubjects", new { Area = AreasConsts.Admin });
        }
    }
}