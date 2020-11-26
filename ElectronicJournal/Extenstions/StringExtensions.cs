using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Extenstions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            return false;
        }
    }
}
