using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Academic.HomeWorks.Dto
{
    public class UpdateHomeWorkInput
    {
        public long HomeWorkId { get; set; }
        public string Description { get; set; }
        public byte[] HomeWorkData { get; set; }
    }
}
