using ElectronicJournal.Application.Authorization.Users.Dto.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Student.Models.Rating
{
    public class StudentRatingViewModel
    {
        public StudentItemDto Student { get; set; }
        public float Score { get; set; }
    }
}
