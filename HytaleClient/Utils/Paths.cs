using System;
using System.IO;
using System.Reflection;

namespace HytaleClient.Utils
{
	// Token: 0x020007C9 RID: 1993
	public static class Paths
	{
		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x00052828 File Offset: 0x00050A28
		// (set) Token: 0x060033DE RID: 13278 RVA: 0x0005282F File Offset: 0x00050A2F
		public static string UserData { get; private set; }

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x00052837 File Offset: 0x00050A37
		// (set) Token: 0x060033E0 RID: 13280 RVA: 0x0005283E File Offset: 0x00050A3E
		public static string CachedAssets { get; private set; }

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060033E1 RID: 13281 RVA: 0x00052846 File Offset: 0x00050A46
		// (set) Token: 0x060033E2 RID: 13282 RVA: 0x0005284D File Offset: 0x00050A4D
		public static string Saves { get; private set; }

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x00052855 File Offset: 0x00050A55
		// (set) Token: 0x060033E4 RID: 13284 RVA: 0x0005285C File Offset: 0x00050A5C
		public static string TempAssetDownload { get; private set; }

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x00052864 File Offset: 0x00050A64
		// (set) Token: 0x060033E6 RID: 13286 RVA: 0x0005286B File Offset: 0x00050A6B
		public static string TempAssetEditorDownload { get; private set; }

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x00052873 File Offset: 0x00050A73
		// (set) Token: 0x060033E8 RID: 13288 RVA: 0x0005287A File Offset: 0x00050A7A
		public static string SharedData { get; private set; }

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x00052882 File Offset: 0x00050A82
		// (set) Token: 0x060033EA RID: 13290 RVA: 0x00052889 File Offset: 0x00050A89
		public static string GameData { get; private set; }

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x00052891 File Offset: 0x00050A91
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x00052898 File Offset: 0x00050A98
		public static string EditorData { get; private set; }

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000528A0 File Offset: 0x00050AA0
		// (set) Token: 0x060033EE RID: 13294 RVA: 0x000528A7 File Offset: 0x00050AA7
		public static string CoherentUI { get; private set; }

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x000528AF File Offset: 0x00050AAF
		// (set) Token: 0x060033F0 RID: 13296 RVA: 0x000528B6 File Offset: 0x00050AB6
		public static string MonacoEditor { get; private set; }

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060033F1 RID: 13297 RVA: 0x000528BE File Offset: 0x00050ABE
		// (set) Token: 0x060033F2 RID: 13298 RVA: 0x000528C5 File Offset: 0x00050AC5
		public static string Language { get; private set; }

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000528CD File Offset: 0x00050ACD
		// (set) Token: 0x060033F4 RID: 13300 RVA: 0x000528D4 File Offset: 0x00050AD4
		public static string Java { get; private set; }

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000528DC File Offset: 0x00050ADC
		// (set) Token: 0x060033F6 RID: 13302 RVA: 0x000528E3 File Offset: 0x00050AE3
		public static string Server { get; private set; }

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x060033F7 RID: 13303 RVA: 0x000528EB File Offset: 0x00050AEB
		// (set) Token: 0x060033F8 RID: 13304 RVA: 0x000528F2 File Offset: 0x00050AF2
		public static string BuiltInAssets { get; private set; }

		// Token: 0x060033F9 RID: 13305 RVA: 0x000528FC File Offset: 0x00050AFC
		public static void Setup()
		{
			Paths.Java = OptionsHelper.JavaExecutable;
			Paths.Server = OptionsHelper.ServerJar;
			Paths.BuiltInAssets = OptionsHelper.AssetsDirectory;
			Paths.UserData = OptionsHelper.UserDataDirectory;
			Paths.CachedAssets = Path.Combine(Paths.UserData, "CachedAssets");
			Paths.Saves = Path.Combine(Paths.UserData, "Saves");
			Paths.TempAssetDownload = Path.Combine(Paths.UserData, "AssetDownload.tmp");
			Paths.TempAssetEditorDownload = Path.Combine(Paths.UserData, "AssetEditorDownload.tmp");
			string dataDirectory = OptionsHelper.DataDirectory;
			Paths.SharedData = Path.Combine(dataDirectory, "Shared");
			Paths.GameData = Path.Combine(dataDirectory, "Game");
			Paths.EditorData = Path.Combine(dataDirectory, "Editor");
			Paths.CoherentUI = Path.Combine(Paths.GameData, "CoherentUI");
			Paths.MonacoEditor = Path.Combine(Paths.SharedData, "MonacoEditor");
			Paths.Language = Path.Combine(Paths.SharedData, "Language");
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00052A04 File Offset: 0x00050C04
		public static string TrimBackslash(string serverPath)
		{
			bool flag = serverPath.EndsWith("\\");
			if (flag)
			{
				serverPath = serverPath.Substring(0, serverPath.Length - 1);
			}
			return serverPath;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00052A38 File Offset: 0x00050C38
		public static string EnsureUniqueDirname(string dirname)
		{
			int num = 0;
			string text;
			for (;;)
			{
				text = dirname;
				bool flag = num > 0;
				if (flag)
				{
					text += string.Format(".{0}", num);
				}
				bool flag2 = !Directory.Exists(text);
				if (flag2)
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00052A8C File Offset: 0x00050C8C
		public static string EnsureUniqueFilename(string filename, string extension)
		{
			int num = 0;
			string text;
			for (;;)
			{
				text = filename;
				bool flag = num > 0;
				if (flag)
				{
					text += string.Format(".{0}", num);
				}
				text += extension;
				bool flag2 = !File.Exists(text);
				if (flag2)
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x00052AE8 File Offset: 0x00050CE8
		public static string StripBasePath(string path, string basePath)
		{
			return path.StartsWith(basePath) ? path.Substring(basePath.Length) : path;
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x00052B14 File Offset: 0x00050D14
		public static bool IsSubPathOf(string path, string baseDirPath)
		{
			string text = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(path));
			bool flag = !text.EndsWith("/");
			if (flag)
			{
				text += "/";
			}
			string text2 = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(baseDirPath));
			bool flag2 = !text2.EndsWith("/");
			if (flag2)
			{
				text2 += "/";
			}
			return text.StartsWith(text2, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04001742 RID: 5954
		public static readonly string App = Path.GetDirectoryName(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));
	}
}
