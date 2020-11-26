using ElectronicJournal.Application.Academic.AcademicSubjects.Dto;
using ElectronicJournal.Application.Academic.HomeWorks.Dto;
using ElectronicJournal.Application.Academic.StudyGroups.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.HomeWorks
{
    public class EditInfoHomeWorkViewModel
    {
        public UpdateHomeWorkInput Input { get; set; }
        public string ReturnUrl { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile HomeWorkFile { get; set; }
        public AcademicSubjectSimpleItemDto Subject { get; set; }
        public StudyGroupSimpleItemDto StudyGroup { get; set; }
        public EditInfoHomeWorkViewModel()
        {
            Input = new UpdateHomeWorkInput();
        }
    }
}
