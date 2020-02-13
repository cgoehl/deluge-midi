using System;
using System.Collections.Generic;
using System.Linq;

namespace DelugeMidi
{
	class Layout
	{

		public Layout(string template)
		{
			Contents = template.Split(Environment.NewLine)
				.Select(row => row.Split(",").ToArray())
				.ToArray();
			Height = Contents.Length;
			Width = Contents[0].Length;
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