using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using HytaleClient.Application.Services;
using HytaleClient.Data.Boot;
using NDesk.Options;

namespace HytaleClient.Utils
{
	// Token: 0x020007C8 RID: 1992
	internal static class OptionsHelper
	{
		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x0600339C RID: 13212 RVA: 0x00051BA7 File Offset: 0x0004FDA7
		// (set) Token: 0x0600339D RID: 13213 RVA: 0x00051BAE File Offset: 0x0004FDAE
		public static ServicesEndpoint Endpoint { get; private set; } = ServicesEndpoint.Default;

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x00051BB6 File Offset: 0x0004FDB6
		// (set) Token: 0x0600339F RID: 13215 RVA: 0x00051BBD File Offset: 0x0004FDBD
		public static string ServerAddress { get; private set; }

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x00051BC5 File Offset: 0x0004FDC5
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x00051BCC File Offset: 0x0004FDCC
		public static string WorldName { get; private set; }

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x00051BD4 File Offset: 0x0004FDD4
		// (set) Token: 0x060033A3 RID: 13219 RVA: 0x00051BDB File Offset: 0x0004FDDB
		public static bool AutoProfiling { get; private set; }

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x060033A4 RID: 13220 RVA: 0x00051BE3 File Offset: 0x0004FDE3
		// (set) Token: 0x060033A5 RID: 13221 RVA: 0x00051BEA File Offset: 0x0004FDEA
		public static bool DisableAffinity { get; private set; }

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x060033A6 RID: 13222 RVA: 0x00051BF2 File Offset: 0x0004FDF2
		// (set) Token: 0x060033A7 RID: 13223 RVA: 0x00051BF9 File Offset: 0x0004FDF9
		public static string UserDataDirectory { get; private set; }

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x060033A8 RID: 13224 RVA: 0x00051C01 File Offset: 0x0004FE01
		// (set) Token: 0x060033A9 RID: 13225 RVA: 0x00051C08 File Offset: 0x0004FE08
		public static string DataDirectory { get; private set; }

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060033AA RID: 13226 RVA: 0x00051C10 File Offset: 0x0004FE10
		// (set) Token: 0x060033AB RID: 13227 RVA: 0x00051C17 File Offset: 0x0004FE17
		public static string JavaExecutable { get; private set; }

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060033AC RID: 13228 RVA: 0x00051C1F File Offset: 0x0004FE1F
		// (set) Token: 0x060033AD RID: 13229 RVA: 0x00051C26 File Offset: 0x0004FE26
		public static string AssetsDirectory { get; private set; }

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060033AE RID: 13230 RVA: 0x00051C2E File Offset: 0x0004FE2E
		// (set) Token: 0x060033AF RID: 13231 RVA: 0x00051C35 File Offset: 0x0004FE35
		public static string ServerJar { get; private set; }

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x00051C3D File Offset: 0x0004FE3D
		// (set) Token: 0x060033B1 RID: 13233 RVA: 0x00051C44 File Offset: 0x0004FE44
		public static IReadOnlyList<string> CustomServerArgumentList { get; private set; } = new List<string>();

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x00051C4C File Offset: 0x0004FE4C
		// (set) Token: 0x060033B3 RID: 13235 RVA: 0x00051C53 File Offset: 0x0004FE53
		public static string CertificatePath { get; private set; }

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x00051C5B File Offset: 0x0004FE5B
		// (set) Token: 0x060033B5 RID: 13237 RVA: 0x00051C62 File Offset: 0x0004FE62
		public static string PrivateKeyPath { get; private set; }

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060033B6 RID: 13238 RVA: 0x00051C6A File Offset: 0x0004FE6A
		// (set) Token: 0x060033B7 RID: 13239 RVA: 0x00051C71 File Offset: 0x0004FE71
		public static bool IsUiDevEnabled { get; private set; }

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x00051C79 File Offset: 0x0004FE79
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x00051C80 File Offset: 0x0004FE80
		public static string LogFileOverride { get; private set; }

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x00051C88 File Offset: 0x0004FE88
		// (set) Token: 0x060033BB RID: 13243 RVA: 0x00051C8F File Offset: 0x0004FE8F
		public static bool LaunchEditor { get; private set; }

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x00051C97 File Offset: 0x0004FE97
		// (set) Token: 0x060033BD RID: 13245 RVA: 0x00051C9E File Offset: 0x0004FE9E
		public static string OpenAssetPath { get; private set; }

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x00051CA6 File Offset: 0x0004FEA6
		// (set) Token: 0x060033BF RID: 13247 RVA: 0x00051CAD File Offset: 0x0004FEAD
		public static string OpenAssetId { get; private set; }

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x00051CB5 File Offset: 0x0004FEB5
		// (set) Token: 0x060033C1 RID: 13249 RVA: 0x00051CBC File Offset: 0x0004FEBC
		public static string OpenAssetType { get; private set; }

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x00051CC4 File Offset: 0x0004FEC4
		// (set) Token: 0x060033C3 RID: 13251 RVA: 0x00051CCB File Offset: 0x0004FECB
		public static bool OpenCosmetics { get; private set; }

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x00051CD3 File Offset: 0x0004FED3
		// (set) Token: 0x060033C5 RID: 13253 RVA: 0x00051CDA File Offset: 0x0004FEDA
		public static float AutoReconnectDelay { get; private set; }

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x00051CE2 File Offset: 0x0004FEE2
		// (set) Token: 0x060033C7 RID: 13255 RVA: 0x00051CE9 File Offset: 0x0004FEE9
		public static string InsecureUsername { get; private set; }

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x00051CF1 File Offset: 0x0004FEF1
		// (set) Token: 0x060033C9 RID: 13257 RVA: 0x00051CF8 File Offset: 0x0004FEF8
		public static bool DisableCharacterAtlasCompression { get; private set; }

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060033CA RID: 13258 RVA: 0x00051D00 File Offset: 0x0004FF00
		// (set) Token: 0x060033CB RID: 13259 RVA: 0x00051D07 File Offset: 0x0004FF07
		public static bool GenerateUIDocs { get; private set; }

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x00051D0F File Offset: 0x0004FF0F
		// (set) Token: 0x060033CD RID: 13261 RVA: 0x00051D16 File Offset: 0x0004FF16
		public static bool DisableServices { get; private set; }

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060033CE RID: 13262 RVA: 0x00051D1E File Offset: 0x0004FF1E
		// (set) Token: 0x060033CF RID: 13263 RVA: 0x00051D25 File Offset: 0x0004FF25
		private static string WorkspacesDirectory { get; set; }

		// Token: 0x060033D0 RID: 13264 RVA: 0x00051D30 File Offset: 0x0004FF30
		public static bool Setup(string[] args)
		{
			bool showHelp = false;
			OptionSet optionSet = new OptionSet();
			optionSet.Add("endpoint=", "Sets the endpoint to use for auth/services", delegate(string v)
			{
				OptionsHelper.Endpoint = ServicesEndpoint.Parse(v);
			});
			optionSet.Add("s|server=", "Connect to the specified server", delegate(string v)
			{
				OptionsHelper.ServerAddress = v;
			});
			optionSet.Add("w|world=", "Load the specified world", delegate(string v)
			{
				OptionsHelper.WorldName = v;
			});
			optionSet.Add("p|profiling", "Show the debug screen by default and run the command '.profiling on all' when joining a game", delegate(string v)
			{
				OptionsHelper.AutoProfiling = (v != null);
			});
			optionSet.Add("disableaffinity", "Disables setting the process affinity", delegate(string v)
			{
				OptionsHelper.DisableAffinity = (v != null);
			});
			optionSet.Add("data-dir=", "Path to the game's data directory", delegate(string v)
			{
				OptionsHelper.DataDirectory = v;
			});
			optionSet.Add("user-dir=", "Path to the user's data directory", delegate(string v)
			{
				OptionsHelper.UserDataDirectory = v;
			});
			optionSet.Add("java-exec=", "Path to the Java executable used for starting the server in single-player", delegate(string v)
			{
				OptionsHelper.JavaExecutable = v;
			});
			optionSet.Add("assets-dir=", "Path to the assets directory", delegate(string v)
			{
				OptionsHelper.AssetsDirectory = v;
			});
			optionSet.Add("server-jar=", "Path to the server JAR executable", delegate(string v)
			{
				OptionsHelper.ServerJar = v;
			});
			optionSet.Add("cert=", "Path to the client's certificate", delegate(string v)
			{
				OptionsHelper.CertificatePath = v;
			});
			optionSet.Add("key=", "Path to the client's private key", delegate(string v)
			{
				OptionsHelper.PrivateKeyPath = v;
			});
			optionSet.Add("log-file=", "Override the path used for the log file", delegate(string v)
			{
				OptionsHelper.LogFileOverride = v;
			});
			optionSet.Add("editor", "Launches the editor", delegate(string v)
			{
				OptionsHelper.LaunchEditor = (v != null);
			});
			optionSet.Add("open-asset-path=", "Opens the asset with the specified path in the asset editor", delegate(string v)
			{
				OptionsHelper.OpenAssetPath = v;
			});
			optionSet.Add("open-asset-id=", "Opens the asset with the specified id in the asset editor", delegate(string v)
			{
				OptionsHelper.OpenAssetId = v;
			});
			optionSet.Add("open-asset-type=", "Opens the asset with the specified type in the asset editor", delegate(string v)
			{
				OptionsHelper.OpenAssetType = v;
			});
			optionSet.Add("open-cosmetics", "Launches the cosmetics editor without opening an asset", delegate(string v)
			{
				OptionsHelper.OpenCosmetics = (v != null);
			});
			optionSet.Add("ui-dev", "If specified, UI development mode will be enabled", delegate(string v)
			{
				OptionsHelper.IsUiDevEnabled = (v != null);
			});
			optionSet.Add("dev-workspace-dir=", "Root directory containing the cloned client/server/assets repositories", delegate(string v)
			{
				OptionsHelper.WorkspacesDirectory = v;
			});
			optionSet.Add("auto-reconnect-delay=", "Enables auto-reconnect and sets the delay in milliseconds", delegate(string v)
			{
				OptionsHelper.AutoReconnectDelay = float.Parse(v, CultureInfo.InvariantCulture);
			});
			optionSet.Add("insecure-username=", "Sets the username for insecure auth", delegate(string v)
			{
				OptionsHelper.InsecureUsername = v;
			});
			optionSet.Add("disable-characteratlas-compression=", "Disables compression for the character atlas", delegate(string v)
			{
				OptionsHelper.DisableCharacterAtlasCompression = (v != null);
			});
			optionSet.Add("generate-ui-docs", "Generates UI documentation files and quits immediately", delegate(string v)
			{
				OptionsHelper.GenerateUIDocs = (v != null);
			});
			optionSet.Add("disable-services=", "Disables Hytale services", delegate(string v)
			{
				OptionsHelper.DisableServices = (v != null);
			});
			optionSet.Add("h|help", "Show this message and exit", delegate(string v)
			{
				showHelp = (v != null);
			});
			OptionSet optionSet2 = optionSet;
			try
			{
				List<string> list = optionSet2.Parse(args);
				bool flag = list.Count > 0;
				if (flag)
				{
					OptionsHelper.LoadLegacyPayload(string.Join(" ", list));
				}
			}
			catch (Exception e)
			{
				return OptionsHelper.ShowParseError(e);
			}
			bool showHelp2 = showHelp;
			bool result;
			if (showHelp2)
			{
				result = OptionsHelper.ShowHelp(optionSet2);
			}
			else
			{
				OptionsHelper.SetDevelopmentDefaults();
				result = OptionsHelper.ValidateArguments();
			}
			return result;
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00052290 File Offset: 0x00050490
		private static bool ValidateArguments()
		{
			try
			{
				OptionsHelper.ValidateDirectory(OptionsHelper.DataDirectory, "Data directory", true);
				OptionsHelper.ValidateDirectory(OptionsHelper.AssetsDirectory, "Assets directory", true);
				OptionsHelper.CreateDirectory(OptionsHelper.UserDataDirectory, "User data directory", false);
				OptionsHelper.OpenFile(OptionsHelper.JavaExecutable, "Java executable", false);
				OptionsHelper.OpenFile(OptionsHelper.ServerJar, "Server JAR file", false);
				bool flag = OptionsHelper.LogFileOverride != null;
				if (flag)
				{
					OptionsHelper.ValidateDirectory(Path.GetDirectoryName(OptionsHelper.LogFileOverride), "Log file directory", true);
				}
			}
			catch (Exception e)
			{
				return OptionsHelper.ShowParseError(e);
			}
			OptionsHelper.OpenFile(OptionsHelper.CertificatePath, "Client certificate", false);
			OptionsHelper.OpenFile(OptionsHelper.PrivateKeyPath, "Client private key", false);
			return true;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x00052360 File Offset: 0x00050560
		[Obsolete]
		private static void LoadLegacyPayload(string json)
		{
			LegacyBootPayload legacyBootPayload = LegacyBootPayload.Parse(json);
			OptionsHelper.AssetsDirectory = legacyBootPayload.AssetsDirectory;
			OptionsHelper.ServerJar = legacyBootPayload.ServerJar;
			OptionsHelper.CustomServerArgumentList = legacyBootPayload.CustomServerArguments.Split(new char[]
			{
				' '
			});
			OptionsHelper.JavaExecutable = legacyBootPayload.JavaExecutable;
			bool flag = OptionsHelper.DataDirectory == null;
			if (flag)
			{
				OptionsHelper.DataDirectory = Path.Combine(Paths.App, "Data");
			}
			bool flag2 = OptionsHelper.UserDataDirectory == null;
			if (flag2)
			{
				OptionsHelper.UserDataDirectory = Path.Combine(Paths.App, OptionsHelper.LaunchEditor ? "EditorUserData" : "UserData");
			}
			OptionsHelper.LoadLegacyCredentials();
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x00052410 File Offset: 0x00050610
		private static void OpenFile(string file, string name, bool required = true)
		{
			bool flag = required && OptionsHelper.ValidateOption(file, name, required) && !File.Exists(file);
			if (flag)
			{
				throw new FileNotFoundException(name + " is set to a file that doesn't exist: " + file);
			}
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x00052450 File Offset: 0x00050650
		private static bool ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: " + OptionsHelper.ExeName + " [OPTIONS]+");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
			return false;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0005249C File Offset: 0x0005069C
		private static bool ShowParseError(Exception e)
		{
			Console.Write(OptionsHelper.ExeName + ": ");
			Console.WriteLine(e.Message);
			Console.WriteLine("Try '" + OptionsHelper.ExeName + " --help' for more information.");
			return false;
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000524EC File Offset: 0x000506EC
		private static string GetDefaultJavaExecutable()
		{
			return EnvironmentHelper.ResolvePathExecutable((BuildInfo.Platform == Platform.Windows) ? "java.exe" : "java");
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x00052518 File Offset: 0x00050718
		private static void LoadLegacyCredentials()
		{
			try
			{
				string homeDirectory = LegacyLauncher.GetHomeDirectory();
				string text = Path.Combine(homeDirectory, "cert");
				string text2 = Path.Combine(homeDirectory, "privKey");
				bool flag = !File.Exists(text);
				if (flag)
				{
					throw new FileNotFoundException("Could not find certificate file: " + text);
				}
				bool flag2 = !File.Exists(text2);
				if (flag2)
				{
					throw new FileNotFoundException("Could not find private key file: " + text2);
				}
				OptionsHelper.CertificatePath = text;
				OptionsHelper.PrivateKeyPath = text2;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Couldn't load credentials from launcher v1!");
				Console.WriteLine(ex);
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000525C0 File Offset: 0x000507C0
		private static void SetDevelopmentDefaults()
		{
			bool flag = OptionsHelper.WorkspacesDirectory == null;
			if (flag)
			{
				OptionsHelper.WorkspacesDirectory = Path.GetFullPath(Path.Combine(new string[]
				{
					Paths.App,
					"..",
					"..",
					"..",
					"..",
					".."
				}));
			}
			bool flag2 = OptionsHelper.JavaExecutable == null;
			if (flag2)
			{
				OptionsHelper.JavaExecutable = OptionsHelper.GetDefaultJavaExecutable();
			}
			bool flag3 = OptionsHelper.DataDirectory == null;
			if (flag3)
			{
				string fullPath = Path.GetFullPath(Path.Combine(new string[]
				{
					Paths.App,
					"..",
					"..",
					"..",
					".."
				}));
				OptionsHelper.DataDirectory = Path.Combine(fullPath, "Data");
			}
			bool flag4 = OptionsHelper.UserDataDirectory == null;
			if (flag4)
			{
				OptionsHelper.UserDataDirectory = Path.Combine(Paths.App, OptionsHelper.LaunchEditor ? "EditorUserData" : "UserData");
			}
			bool flag5 = OptionsHelper.AssetsDirectory == null;
			if (flag5)
			{
				OptionsHelper.AssetsDirectory = Path.Combine(OptionsHelper.WorkspacesDirectory, "HytaleAssets");
			}
			bool flag6 = OptionsHelper.ServerJar == null;
			if (flag6)
			{
				OptionsHelper.ServerJar = Path.Combine(new string[]
				{
					OptionsHelper.WorkspacesDirectory,
					"HytaleServer",
					"dist",
					"HytaleServer",
					"HytaleServer.jar"
				});
			}
			bool flag7 = OptionsHelper.PrivateKeyPath == null || OptionsHelper.CertificatePath == null;
			if (flag7)
			{
				Console.WriteLine("Incomplete certificate/key pair provided for authentication, will try to find credentials from launcher v1");
				OptionsHelper.LoadLegacyCredentials();
			}
			OptionsHelper.IsUiDevEnabled = true;
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x00052768 File Offset: 0x00050968
		private static void CreateDirectory(string dir, string name, bool required = false)
		{
			bool flag = OptionsHelper.ValidateOption(dir, name, required);
			if (flag)
			{
				Directory.CreateDirectory(dir);
			}
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x0005278C File Offset: 0x0005098C
		private static void ValidateDirectory(string dir, string name, bool required = true)
		{
			bool flag = required && OptionsHelper.ValidateOption(dir, name, required) && !Directory.Exists(dir);
			if (flag)
			{
				throw new DirectoryNotFoundException(name + " is set to non-existent directory: " + dir);
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000527CC File Offset: 0x000509CC
		private static bool ValidateOption(string value, string name, bool required)
		{
			bool flag = value == null;
			bool result;
			if (flag)
			{
				if (required)
				{
					throw new NullReferenceException(name + " not specified");
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x04001741 RID: 5953
		private static readonly string ExeName = AppDomain.CurrentDomain.FriendlyName;
	}
}
