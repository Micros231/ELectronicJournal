using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.StudyGroups
{
    public class GetStudyGroupsViewModel
    {
        public GetStudyGroupsInput Input { get; set; }
        public ListResultDto<StudyGroupItemDto> Value { get; set; }

        public GetStudyGroupsViewModel()
        {
            Input = new GetStudyGroupsInput();
            Value = new ListResultDto<StudyGroupItemDto>();
        }
    }
}
