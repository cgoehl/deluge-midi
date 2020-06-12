using System.IO;
using System.Linq;
using System;

namespace DelugeMidi
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program().run();
		}

		void run()
		{
			var config = new Config();
			var root = FindRoot(new DirectoryInfo(config.DelugeSdPath));
			var dm = new DelugeMidi(root, config);
			dm.Inject();
		}

		DirectoryInfo FindRoot(DirectoryInfo startingPoint)
		{
			Console.WriteLine("Scanning: {0}", startingPoint);

			var directories = startingPoint.EnumerateDirectories().ToArray();
			if (directories.SingleOrDefault(f => f.Name == "SONGS") != null &&
			    directories.SingleOrDefault(f => f.Name == "_delugeTools") != null)
			{
				Console.WriteLine("Found root: {0}", startingPoint);
				return startingPoint;
			}

			if (startingPoint.Equals(startingPoint.Root))
			{
				throw new DirectoryNotFoundException("Could not find Deluge SD root.");
			}

			return FindRoot(startingPoint.Parent);
		}
	}

}
