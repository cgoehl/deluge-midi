using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
			var root = FindRoot(Directory.GetParent(GetType().Assembly.Location));
			var dm = new DelugeMidi(root);
			dm.Inject();

		}

		DirectoryInfo FindRoot(DirectoryInfo startingPoint)
		{
			var directories = startingPoint.EnumerateDirectories().ToArray();
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
