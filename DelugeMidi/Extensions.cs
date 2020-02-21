using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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

		public static void ReplaceChildElement(this XElement parent, XElement newChild)
		{
			var current = parent.Elements(newChild.Name);
			current.Remove();
			parent.Add(newChild);
		}


		public static IEnumerable<string> Split(this string s, string separator) =>
			s.Split(new[] { separator }, StringSplitOptions.None);

		public static IEnumerable<string> SplitLines(this string s) =>
			s.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
}
}