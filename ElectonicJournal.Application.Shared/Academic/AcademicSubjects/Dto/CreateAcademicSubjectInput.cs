using ElectronicJournal.Application.Dto;
using ElectronicJournal.Application.Dto.HasDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjects.Dto
{
    public class CreateAcademicSubjectInput : IHasName
    {
        [Required(ErrorMessage = "Необходимо ввести имя.")]
        public string Name { get; set; }
        public List<ComboboxItemDto> StudyGroupComboboxes { get; set; }
        public CreateAcademicSubjectInput()
        {
            StudyGroupComboboxes = new List<ComboboxItemDto>();
        }
    }
}
