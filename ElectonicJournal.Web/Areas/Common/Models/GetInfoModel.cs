using ElectronicJournal.Application.Authorization.Users.Dto.Teacher;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Common.Models
{
    public class GetInfoModel
    {
        public ListResultDto<TeacherItemDto> Value { get; set; }

        public GetInfoModel()
        {
            Value = new ListResultDto<TeacherItemDto>();
        }
    }
}
