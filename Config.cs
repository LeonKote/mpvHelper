using System;
using System.IO;
using System.Linq;

namespace mpvHelper
{
	public static class Config
	{
		public static string GetPlayerPath()
		{
			if (File.Exists("mpvHelper.yaml"))
			{
				var properties = File.ReadAllLines("mpvHelper.yaml")
					.Select(x => (x.Substring(0, x.IndexOf(':')).Trim(),x.Substring(x.IndexOf(':') + 1).Trim()));
				foreach (var property in properties)
				{
					if (property.Item1 == "playerPath")
						return property.Item2;
				}
			}
			else
				File.WriteAllText("mpvHelper.yaml", $"playerPath: C:\\mpv\\mpv.exe{Environment.NewLine}");

			return "C:\\mpv\\mpv.exe";
		}

		public static void SetPlayerPath(string playerPath)
		{
			File.WriteAllText("mpvHelper.yaml", $"playerPath: {playerPath}{Environment.NewLine}");
		}
	}
}
