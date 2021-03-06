﻿using ElectronicJournal.Application.Dto.HasDto;
using ElectronicJournal.Extenstions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElectronicJournal.Application.Academic.HomeWorks.Dto
{
    public class CreateHomeWorkInput : IHasAcademicSubjectId<long>, IHasStudyGroupIdDto<long>
    {
        public long AcademicSubjectId { get; set; }
        public long StudyGroupId { get; set; }
        public string Description { get; set; }
        public byte[] HomeWorkData { get; set; }
        public string DateString { get; set; }
        public DateTime? EndDate => ParseDate();

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
