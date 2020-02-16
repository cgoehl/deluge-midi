﻿using System.IO;
using System.Linq;
using System;

namespace DelugeMidi
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().run(args);
		}

		void run(string[] args)
		{
			var root = FindRoot(Directory.GetParent(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
			var dm = new DelugeMidi(root);
			dm.Inject();

		}

		DirectoryInfo FindRoot(DirectoryInfo startingPoint)
		{
			var directories = startingPoint.EnumerateDirectories().ToArray();
			Console.WriteLine("Scanning: {0}", startingPoint);
			directories.ForEach(d => Console.WriteLine("  {0}", d.Name));
			if (directories.SingleOrDefault(f => f.Name == "SONGS") != null &&
			    directories.SingleOrDefault(f => f.Name == "_delugeTools") != null)
			{
				return startingPoint;
			}

			if (startingPoint.Equals(startingPoint.Root))
			{
				return null;
			}

			return FindRoot(startingPoint.Parent);
		}
	}

}
