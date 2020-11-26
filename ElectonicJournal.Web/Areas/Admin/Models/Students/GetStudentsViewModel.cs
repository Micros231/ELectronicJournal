using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Students
{
    public class GetStudentsViewModel
    {
        public string ReturnUrl { get; set; }
        public GetStudentsInput Input { get; set; }
        public ListResultDto<StudentItemDto> Value { get; set; }
        public GetStudentsViewModel()
        {
            Input = new GetStudentsInput();
            Value = new ListResultDto<StudentItemDto>();
        }
    }
}
