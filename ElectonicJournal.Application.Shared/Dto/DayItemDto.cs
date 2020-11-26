using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public class DayItemDto 
    {
        public int DayInMonth { get; set; }
        public DateTime DateTime { get; set; }
        public string DateString { get; set; }
    }
}
