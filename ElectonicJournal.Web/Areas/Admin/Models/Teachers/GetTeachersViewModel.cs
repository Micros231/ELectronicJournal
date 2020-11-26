using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Teachers
{
    public class GetTeachersViewModel
    {
        public string ReturnUrl { get; set; }
        public GetTeachersInput Input { get; set; }
        public ListResultDto<TeacherItemDto> Value { get; set; }

        public GetTeachersViewModel()
        {
            Input = new GetTeachersInput();
            Value = new ListResultDto<TeacherItemDto>();
        }
    }
}
