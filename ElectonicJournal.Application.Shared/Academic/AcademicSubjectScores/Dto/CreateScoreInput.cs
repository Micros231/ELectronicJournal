using ElectronicJournal.Application.Dto.HasDto;
using ElectronicJournal.Extenstions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto
{
    public class CreateScoreInput : IHasStudentIdDto<long>, IHasStudyGroupIdDto<long>, IHasAcademicSubjectId<long>, IHasTeacherId<long>
    {
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
        public long StudyGroupId { get; set; }
        public long AcademicSubjectId { get; set; }
        [Range(0, 5, ErrorMessage = "Вы преодолели пределы. Нельзя поставить оценку выше 5 и ниже 0")]
        public int Score { get; set; }
        public string DateString { get; set; }
        public DateTime? Date => ParseDate();

        private DateTime? ParseDate()
        {
            if (!DateString.IsNullOrEmpty())
            {
                if (DateTime.TryParseExact(DateString, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
            }
            return null;
           
        }
    }
}
