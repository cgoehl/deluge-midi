using System.IO;

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
			ControllerKnobsLayout = ReadLayout(Path.Combine(layoutsDir, "controller.csv"));
			ControllerNotesLayout = ReadLayout(Path.Combine(layoutsDir, "controller_notes.csv"));
			FMLayout = ReadLayout(Path.Combine(layoutsDir, "fm.csv"));
			KitColLayout = ReadLayout(Path.Combine(layoutsDir, "kit_col.csv"));
			RingModLayout = ReadLayout(Path.Combine(layoutsDir, "ringmod.csv"));
			SubtractiveLayout = ReadLayout(Path.Combine(layoutsDir, "subtractive.csv"));
		}

		public Layout ControllerNotesLayout { get; }

		public Layout SubtractiveLayout { get; }
		public Layout RingModLayout { get; }
		public Layout KitColLayout { get; }
		public Layout FMLayout { get; }
		public Layout ControllerKnobsLayout { get; }
		public DirectoryInfo SongsDir { get; }

		public void Inject()
		{
			var files = SongsDir.EnumerateFiles("*.XML", SearchOption.TopDirectoryOnly);
			files.ForEach(file =>
			{
				var newDoc = new FileProcessor(_config, this).Process(file.FullName);
				newDoc.Save(file.FullName);
			});
		}

		private Layout ReadLayout(string path)
		{
			return new Layout(File.ReadAllText(path));
		}
	}
}