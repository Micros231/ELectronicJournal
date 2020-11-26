using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto
{
    public class UpdateSocreInfoInput
    {
        public long ScoreId { get; set; }
        [Range(0, 5, ErrorMessage = "Вы преодолели пределы. Нельзя поставить оценку выше 5 и ниже 0")]
        public int Score { get; set; }
    }
}
