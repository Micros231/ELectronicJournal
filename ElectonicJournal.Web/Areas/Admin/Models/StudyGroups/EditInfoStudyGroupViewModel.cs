using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.StudyGroups
{
    public class EditInfoStudyGroupViewModel
    {
        public UpdateStudyGroupInfo Input { get; set; }
        public string ReturnUrl { get; set; }
        public EditInfoStudyGroupViewModel()
        {
            Input = new UpdateStudyGroupInfo();
        }
    }
}
