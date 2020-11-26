using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Student.Models.Rating
{
    public class GetRatingViewModel
    {
        public List<StudentRatingViewModel> StudentRatings { get; set; }
        public GetRatingViewModel()
        {
            StudentRatings = new List<StudentRatingViewModel>();
        }
    }
}
