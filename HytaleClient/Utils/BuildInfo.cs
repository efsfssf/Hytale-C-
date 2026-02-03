using System;
using System.IO;
using System.Reflection;
using NLog;
using SDL2;

namespace HytaleClient.Utils
{
	// Token: 0x020007B6 RID: 1974
	internal static class BuildInfo
	{
		// Token: 0x06003327 RID: 13095 RVA: 0x0004E03C File Offset: 0x0004C23C
		static BuildInfo()
		{
			string text = SDL.SDL_GetPlatform();
			string text2 = text;
			string a = text2;
			if (!(a == "Windows"))
			{
				if (!(a == "Mac OS X"))
				{
					if (!(a == "Linux"))
					{
						BuildInfo.Logger.Warn("The platform {0} is not supported! Defaulting to Windows.", text);
						BuildInfo.Platform = Platform.Windows;
					}
					else
					{
						BuildInfo.Platform = Platform.Linux;
					}
				}
				else
				{
					BuildInfo.Platform = Platform.MacOS;
				}
			}
			else
			{
				BuildInfo.Platform = Platform.Windows;
			}
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string arg = "0-dev";
			BuildInfo.Version = string.Format("{0}.{1}.{2}", version.Major, version.Minor, arg);
			bool flag = BuildInfo.RevisionId == null;
			if (flag)
			{
				string text3 = Path.GetFullPath(Path.Combine(new string[]
				{
					Paths.App,
					"..",
					"..",
					"..",
					"..",
					".git"
				}));
				string gitPath = text3;
				bool flag2 = File.Exists(text3) && (File.GetAttributes(text3) & FileAttributes.Directory) == (FileAttributes)0;
				if (flag2)
				{
					string text4 = File.ReadAllLines(text3)[0];
					bool flag3 = !text4.StartsWith("gitdir: ");
					if (flag3)
					{
						throw new Exception("Can't handle work-tree. Missing gitdir");
					}
					text3 = text4.Substring("gitdir: ".Length);
					gitPath = Path.GetFullPath(Path.Combine(text3, "..", ".."));
				}
				string text5 = Path.Combine(text3, "HEAD");
				string text6 = File.ReadAllLines(text5)[0];
				bool flag4 = text6.StartsWith("ref: ");
				if (flag4)
				{
					string text7 = text6.Substring("ref: ".Length);
					BuildInfo.BranchName = text7.Substring(text7.LastIndexOf("/", StringComparison.Ordinal) + 1);
					BuildInfo.RevisionId = BuildInfo.GetShaFromBranch(gitPath, text7, BuildInfo.BranchName);
				}
				else
				{
					BuildInfo.RevisionId = text6;
					BuildInfo.BranchName = "(detached)";
				}
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x0004E270 File Offset: 0x0004C470
		private static string GetShaFromBranch(string gitPath, string headRef, string branchName)
		{
			string text = Path.Combine(gitPath, "refs", "heads", branchName);
			bool flag = File.Exists(text);
			string result;
			if (flag)
			{
				result = File.ReadAllLines(text)[0];
			}
			else
			{
				foreach (string text2 in File.ReadAllLines(Path.Combine(gitPath, "packed-refs")))
				{
					bool flag2 = text2.Contains(headRef);
					if (flag2)
					{
						return text2.Split(new char[]
						{
							' '
						})[0];
					}
				}
				result = "(unknown)";
			}
			return result;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x0004E304 File Offset: 0x0004C504
		public static void PrintAll()
		{
			BuildInfo.Logger.Info("HytaleClient v{0} ({1} {2} {3} — {4} - .NET {5})", new object[]
			{
				BuildInfo.Version,
				BuildInfo.Platform,
				BuildInfo.Architecture,
				BuildInfo.Configuration,
				Environment.OSVersion.VersionString,
				Environment.Version
			});
			BuildInfo.Logger.Info<string, string>("Branch: {0}, Revision: {1}", BuildInfo.BranchName, BuildInfo.RevisionId);
		}

		// Token: 0x040016F2 RID: 5874
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040016F3 RID: 5875
		public static readonly Platform Platform;

		// Token: 0x040016F4 RID: 5876
		public static readonly string Architecture = (IntPtr.Size == 8) ? "x64" : "x86";

		// Token: 0x040016F5 RID: 5877
		public static readonly string Configuration = "Debug";

		// Token: 0x040016F6 RID: 5878
		public static readonly string Version;

		// Token: 0x040016F7 RID: 5879
		public static readonly string RevisionId;

		// Token: 0x040016F8 RID: 5880
		public static readonly string BranchName;
	}
}
