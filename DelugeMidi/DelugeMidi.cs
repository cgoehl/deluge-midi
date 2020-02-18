using System.IO;
using DelugeMidi.FW;

namespace DelugeMidi
{
	class DelugeMidi
	{
		private readonly Config _config;

		public DelugeMidi(DirectoryInfo root, Config config)
		{
			_config = config;
			SongsDir = new DirectoryInfo(Path.Combine(root.FullName, "SONGS"));
			var layoutsDir = Path.Combine(root.FullName, "_delugeTools", "DelugeMidiLayouts");
			ControllerLayout = ReadLayout(Path.Combine(layoutsDir, "controller.csv"));
			FMLayout = ReadLayout(Path.Combine(layoutsDir, "fm.csv"));
			KitColLayout = ReadLayout(Path.Combine(layoutsDir, "kit_col.csv"));
			RingModLayout = ReadLayout(Path.Combine(layoutsDir, "ringmod.csv"));
			SubtractiveLayout = ReadLayout(Path.Combine(layoutsDir, "subtractive.csv"));
		}

		public Layout SubtractiveLayout { get; }
		public Layout RingModLayout { get; }
		public Layout KitColLayout { get; }
		public Layout FMLayout { get; }
		public Layout ControllerLayout { get; }
		public DirectoryInfo SongsDir { get; }

		public void Inject()
		{
			var files = SongsDir.EnumerateFiles("*.XML", SearchOption.TopDirectoryOnly);
			files.ForEach(file =>
			{
				var newDoc = new FileProcessor(_config).Process(file.FullName, this);
				newDoc.Save(file.FullName);
			});
		}

		private Layout ReadLayout(string path)
		{
			return new Layout(File.ReadAllText(path));
		}
	}
}