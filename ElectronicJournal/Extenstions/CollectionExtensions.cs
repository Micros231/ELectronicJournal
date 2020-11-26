using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Extenstions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }
            if (collection.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
