using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NLog;

namespace HytaleClient.Utils
{
	// Token: 0x020007C4 RID: 1988
	public static class Language
	{
		// Token: 0x0600337C RID: 13180 RVA: 0x0004FDA4 File Offset: 0x0004DFA4
		public static void Initialize()
		{
			Language.Logger.Info("System language: {0}", Language.SystemLanguage);
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x0004FDF4 File Offset: 0x0004DFF4
		public static Dictionary<string, string> GetAvailableLanguages()
		{
			IEnumerable<DirectoryInfo> enumerable = new DirectoryInfo(Paths.Language).EnumerateDirectories("*.*", 0);
			return Enumerable.ToDictionary<DirectoryInfo, string, string>(enumerable, (DirectoryInfo language) => language.Name, (DirectoryInfo language) => Language.Parse(File.ReadAllLines(Path.Combine(language.FullName, "meta.lang")))["name"]);
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x0004FE60 File Offset: 0x0004E060
		public static IDictionary<string, string> Parse(string[] lines)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = false;
			string key = null;
			foreach (string text in lines)
			{
				bool flag2 = string.IsNullOrEmpty(text) || text.StartsWith("#");
				if (!flag2)
				{
					bool flag3 = !text.Contains("=") && !flag;
					if (!flag3)
					{
						string text2 = text;
						bool flag4 = text2.EndsWith("\\");
						if (flag4)
						{
							text2 = text2.Substring(0, text2.Length - 1);
						}
						bool flag5 = flag;
						if (flag5)
						{
							dictionary[key] = dictionary[key] + "\n" + text2;
						}
						else
						{
							int num = text.IndexOf("=");
							key = text2.Substring(0, num).Trim();
							text2 = text2.Substring(num + 1).Trim();
							dictionary[key] = text2;
						}
						flag = text.EndsWith("\\");
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x0004FF80 File Offset: 0x0004E180
		private static void LoadLanguageFiles(string language, Dictionary<string, string> dict, string path, string prefix = "")
		{
			path = Path.Combine(path, language);
			bool flag = !Directory.Exists(path);
			if (flag)
			{
				Language.Logger.Warn("Could not load language files from \"{0}\" as the directory doesn't exist.", path);
			}
			else
			{
				IEnumerable<string> enumerable = Directory.EnumerateFiles(path, "*.lang", 1);
				Uri uri = new Uri(path + Path.DirectorySeparatorChar.ToString());
				foreach (string text in enumerable)
				{
					string text2 = uri.MakeRelativeUri(new Uri(text)).ToString();
					string str = prefix + text2.Replace("/", ".").Substring(0, text2.Length - ".lang".Length) + ".";
					foreach (KeyValuePair<string, string> keyValuePair in Language.Parse(File.ReadAllLines(text)))
					{
						dict[str + keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000500D0 File Offset: 0x0004E2D0
		public static string GetAutomaticLanguage()
		{
			string systemLanguage = Language.SystemLanguage;
			return Language.LanguageExists(systemLanguage) ? systemLanguage : Language.GetFallback(systemLanguage);
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000500FC File Offset: 0x0004E2FC
		public static bool LanguageExists(string language)
		{
			return Directory.Exists(Path.Combine(Paths.Language, language));
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x00050120 File Offset: 0x0004E320
		public static IDictionary<string, string> LoadServerLanguageFile(string filename, string language)
		{
			IDictionary<string, string> dictionary = Language.Parse(File.ReadAllLines(Path.Combine(Paths.BuiltInAssets, "Server/Languages/" + Language.DefaultLanguage + "/" + filename)));
			bool flag = language == null;
			if (flag)
			{
				language = Language.GetAutomaticLanguage();
			}
			string text = Path.Combine(Paths.BuiltInAssets, "Server/Languages/" + language + "/" + filename);
			bool flag2 = language != Language.DefaultLanguage && File.Exists(text);
			if (flag2)
			{
				foreach (KeyValuePair<string, string> keyValuePair in Language.Parse(File.ReadAllLines(text)))
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x00050204 File Offset: 0x0004E404
		public static Dictionary<string, string> LoadLanguage(string language)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"currentLocale",
					Language.DefaultLanguage
				}
			};
			Language.LoadLanguageFiles(Language.DefaultLanguage, dictionary, Paths.Language, "ui.");
			Language.LoadLanguageFiles(Language.DefaultLanguage, dictionary, Path.Combine(Paths.BuiltInAssets, "Common", "Languages"), "");
			bool flag = language == null;
			if (flag)
			{
				language = Language.GetAutomaticLanguage();
			}
			bool flag2 = language != Language.DefaultLanguage;
			if (flag2)
			{
				Language.LoadLanguageFiles(language, dictionary, Paths.Language, "ui.");
				Language.LoadLanguageFiles(language, dictionary, Path.Combine(Paths.BuiltInAssets, "Common", "Languages"), "");
				dictionary["currentLocale"] = language;
			}
			return dictionary;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000502CC File Offset: 0x0004E4CC
		private static string GetFallback(string language)
		{
			IDictionary<string, string> dictionary = Language.Parse(File.ReadAllLines(Path.Combine(Paths.Language, "fallback.lang")));
			string text;
			dictionary.TryGetValue(language, out text);
			return text ?? Language.DefaultLanguage;
		}

		// Token: 0x0400171E RID: 5918
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400171F RID: 5919
		private const string FileExtension = ".lang";

		// Token: 0x04001720 RID: 5920
		public static readonly string DefaultLanguage = "en-US";

		// Token: 0x04001721 RID: 5921
		public static readonly string SystemLanguage = Thread.CurrentThread.CurrentCulture.Name;
	}
}
