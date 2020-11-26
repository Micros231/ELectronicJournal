using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.HomeWorks
{
    public class GetHomeWorksViewModel
    {
        public GetHomeWorksInput Input { get; set; }
        public ListResultDto<HomeWorkItemDto> Value { get; set; }

        public GetHomeWorksViewModel()
        {
            Input = new GetHomeWorksInput();
            Value = new ListResultDto<HomeWorkItemDto>();
        }
    }
}
