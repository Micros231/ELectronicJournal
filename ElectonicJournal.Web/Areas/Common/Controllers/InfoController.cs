using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Web.Areas.Common.Models;
using ElectronicJournal.Web.Areas.Startup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Web.Areas.Common.Controllers
{
    [Area(AreasConsts.Common)]
    [Authorize]
    public class InfoController : Controller
    {
        private readonly ITeacherAppService _teacherService;
        public InfoController(ITeacherAppService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GetInfoModel();
            var result = await _teacherService.GetTeachers(new GetTeachersInput());
            if (result.IsSuccessed)
            {
                model.Value = result.Value;
            }
            return View(model);
        }
    }
}