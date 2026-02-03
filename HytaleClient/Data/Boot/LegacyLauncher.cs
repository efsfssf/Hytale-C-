using System;
using System.IO;
using HytaleClient.Utils;

namespace HytaleClient.Data.Boot
{
	// Token: 0x02000B5E RID: 2910
	[Obsolete]
	public static class LegacyLauncher
	{
		// Token: 0x060059EB RID: 23019 RVA: 0x001BDCE4 File Offset: 0x001BBEE4
		public static string GetHomeDirectory()
		{
			string path;
			switch (BuildInfo.Platform)
			{
			case Platform.Windows:
				path = Environment.ExpandEnvironmentVariables("%APPDATA%");
				break;
			case Platform.MacOS:
				path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support");
				break;
			case Platform.Linux:
				path = Environment.ExpandEnvironmentVariables("%HOME%/.config");
				break;
			default:
				throw new Exception(string.Format("Don't know how to find the user data directory for platform {0}", BuildInfo.Platform));
			}
			return Path.Combine(path, "hytale-launcher");
		}
	}
}
