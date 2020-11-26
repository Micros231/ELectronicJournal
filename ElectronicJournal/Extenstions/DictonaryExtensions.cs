using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Extenstions
{
    public static class DictonaryExtensions
    {
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (!dictionary.TryGetValue(key, out TValue value))
			{
				return default;
			}
			return value;
		}
	}
}
