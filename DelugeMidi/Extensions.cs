using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DelugeMidi
{
	public static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var element in enumerable)
			{
				action(element);
			}
		}

		public static IEnumerable<string> Split(this string s, string separator) =>
			s.Split(new[] { separator }, StringSplitOptions.None);
	}
}