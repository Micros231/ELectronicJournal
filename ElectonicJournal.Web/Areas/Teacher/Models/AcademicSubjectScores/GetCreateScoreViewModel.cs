using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Teacher.Models.AcademicSubjectScores
{
    public class GetCreateScoreViewModel
    {
        public long SubjectId { get; set; }
        public long StudentId { get; set; }
        public long TeacherId { get; set; }
        public long StudyGroupId { get; set; }
        public string DateString { get; set; }
    }
}
