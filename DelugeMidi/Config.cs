using System;
using System.Globalization;
using System.IO;

namespace DelugeMidi
{
	class Config
	{
		public Config()
		{
			var defaultPath = new DirectoryInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).Parent.FullName;
			DelugeSdPath = ReadVariable("CONFIG_SD_PATH", defaultPath);
			FirstChannel = ReadVariable("CONFIG_FIRST_CHANNEL", 0);
		}
		
		public string DelugeSdPath { get; }
		public int FirstChannel { get; }

		private T ReadVariable<T>(string key, T fallback)
		{
			try
			{
				var r = (T) Convert.ChangeType(
					Environment.GetEnvironmentVariable(key),
					typeof(T),
					CultureInfo.InvariantCulture);
				return r == null
					? fallback
					: r;
			}
			catch
			{
				return fallback;
			}
		}
	}
}
