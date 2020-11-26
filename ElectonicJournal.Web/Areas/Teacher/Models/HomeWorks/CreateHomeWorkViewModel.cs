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
    public class CreateHomeWorkViewModel
    {
        public CreateHomeWorkInput Input { get; set; }
        public AcademicSubjectSimpleItemDto Subject { get; set; }
        public StudyGroupSimpleItemDto StudyGroup { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile HomeWorkFile { get; set; }
        public string ReturnUrl { get; set; }

        public CreateHomeWorkViewModel()
        {
            Input = new CreateHomeWorkInput();
            Subject = new AcademicSubjectSimpleItemDto();
            StudyGroup = new StudyGroupSimpleItemDto();
        }
    }
}
