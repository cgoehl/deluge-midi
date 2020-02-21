﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DelugeMidi
{
	class Layout
	{

		public Layout(string template)
		{
			Contents = template
				.SplitLines()
				.Select(row => row
					.Split(",")
					.Select(cell => cell.Trim())
					.ToArray())
				.ToArray();
			Height = Contents.Length;
			Width = Contents[0].Length;
			Console.WriteLine(template);
			Console.WriteLine(string.Join(";", Page()));
		}

		private string[][] Contents { get; }
		public int Width { get; }
		public int Height { get; }

		public IEnumerable<string> Page() => Contents.SelectMany(row => row);
		public string At(int x, int y) => Contents[y][x];
		public IEnumerable<string> Row(int y) => Contents[y];
		public IEnumerable<string> Column(int x) => Enumerable.Range(0, Height).Select(y => At(x, y));
	}
}