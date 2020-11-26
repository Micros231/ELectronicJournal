using ElectronicJournal.Application.Dto.HasDto;
using ElectronicJournal.Extenstions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElectronicJournal.Application.Academic.AcademicSubjectScores.Dto
{
    public class GetScoresInput : IHasStudyGroupIdDto<long?>, IHasStudentIdDto<long?>, IHasTeacherId<long?>
    {
        public long? StudyGroupId { get; set; }
        public long? TeacherId { get; set; }
        public long? StudentId { get; set; }
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
