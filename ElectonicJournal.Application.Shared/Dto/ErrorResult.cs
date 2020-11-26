using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public class ErrorResult
    {
        public string Message { get; set; }

        public ErrorResult()
        {

        }
        public ErrorResult(string message)
        {
            Message = message;
        }
    }
}
