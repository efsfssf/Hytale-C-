using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000ACE RID: 2766
	internal class AssetEditorSettings
	{
		// Token: 0x06005754 RID: 22356 RVA: 0x001A7333 File Offset: 0x001A5533
		public static void Migrate(JObject jObject, int version)
		{
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001A7338 File Offset: 0x001A5538
		public void Initialize()
		{
			bool flag = this.AssetsPath == null;
			if (flag)
			{
				this.InitializeAssetsPath();
			}
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001A735C File Offset: 0x001A555C
		private void InitializeAssetsPath()
		{
			bool flag = File.Exists(Path.GetFullPath(Path.Combine(Paths.BuiltInAssets, "CommonAssetsIndex.hashes")));
			if (flag)
			{
				this.DisplayDefaultAssetPathWarning = true;
				this.AssetsPath = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HytaleAssets"));
				AssetEditorSettings.Logger.Info("Initializing asset editor path with default directory at " + this.AssetsPath);
			}
			else
			{
				this.DisplayDefaultAssetPathWarning = false;
				this.AssetsPath = Paths.BuiltInAssets;
				AssetEditorSettings.Logger.Info("Initializing asset editor path with current assets directory at " + this.AssetsPath);
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x001A73F8 File Offset: 0x001A55F8
		public void Load()
		{
			string text = Path.Combine(Paths.UserData, "AssetEditorSettings.json");
			bool flag = !File.Exists(text);
			if (flag)
			{
				this.InitializeAssetsPath();
				this.Save();
			}
			else
			{
				JObject jobject;
				try
				{
					jobject = JObject.Parse(File.ReadAllText(text));
				}
				catch (Exception exception)
				{
					AssetEditorSettings.Logger.Error(exception, "Failed to load asset editor settings json");
					this.InitializeAssetsPath();
					this.Save();
					return;
				}
				this.UncollapsedDirectories = jobject["UncollapsedDirectories"].ToObject<List<string>>();
				bool flag2 = jobject.ContainsKey("ActiveAssetTree");
				if (flag2)
				{
					this.ActiveAssetTree = (AssetTreeFolder)Enum.Parse(typeof(AssetTreeFolder), (string)jobject["ActiveAssetTree"]);
				}
				bool flag3 = jobject.ContainsKey("AssetsPath");
				if (flag3)
				{
					this.AssetsPath = (string)jobject["AssetsPath"];
					this.DisplayDefaultAssetPathWarning = false;
				}
				else
				{
					this.InitializeAssetsPath();
				}
				bool flag4 = jobject.ContainsKey("LastUsedVersion");
				if (flag4)
				{
					this.LastUsedVersion = (string)jobject["LastUsedVersion"];
				}
				bool flag5 = jobject.ContainsKey("PaneSizes");
				if (flag5)
				{
					foreach (KeyValuePair<string, JToken> keyValuePair in ((JObject)jobject["PaneSizes"]))
					{
						AssetEditorSettings.Panes key;
						bool flag6 = !Enum.TryParse<AssetEditorSettings.Panes>(keyValuePair.Key, out key);
						if (!flag6)
						{
							this.PaneSizes[key] = (int)keyValuePair.Value;
						}
					}
				}
			}
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001A75C4 File Offset: 0x001A57C4
		public void Save()
		{
			AssetEditorSettings.<>c__DisplayClass22_0 CS$<>8__locals1 = new AssetEditorSettings.<>c__DisplayClass22_0();
			CS$<>8__locals1.<>4__this = this;
			this._saveCounter++;
			CS$<>8__locals1.version = this._saveCounter;
			JObject jobject = new JObject();
			foreach (KeyValuePair<AssetEditorSettings.Panes, int> keyValuePair in this.PaneSizes)
			{
				jobject[keyValuePair.Key.ToString()] = keyValuePair.Value;
			}
			AssetEditorSettings.<>c__DisplayClass22_0 CS$<>8__locals2 = CS$<>8__locals1;
			JObject jobject2 = new JObject();
			jobject2.Add("UncollapsedDirectories", JArray.FromObject(this.UncollapsedDirectories));
			jobject2.Add("ActiveAssetTree", this.ActiveAssetTree.ToString());
			jobject2.Add("LastUsedVersion", this.LastUsedVersion);
			jobject2.Add("AssetsPath", this.AssetsPath);
			jobject2.Add("PaneSizes", jobject);
			CS$<>8__locals2.data = jobject2;
			Task.Run(delegate()
			{
				CS$<>8__locals1.<>4__this.SaveToFile(CS$<>8__locals1.data, CS$<>8__locals1.version);
			}).ContinueWith(delegate(Task t)
			{
				bool flag = !t.IsFaulted;
				if (!flag)
				{
					AssetEditorSettings.Logger.Error(t.Exception, "Failed to save asset editor settings");
				}
			});
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001A7720 File Offset: 0x001A5920
		private void SaveToFile(JObject data, int count)
		{
			object saveLock = this._saveLock;
			lock (saveLock)
			{
				bool flag2 = this._lastSavedCounter > count;
				if (!flag2)
				{
					string text = Path.Combine(Paths.UserData, "AssetEditorSettings.json");
					File.WriteAllText(text + ".new", data.ToString());
					bool flag3 = File.Exists(text);
					if (flag3)
					{
						File.Replace(text + ".new", text, text + ".bak");
					}
					else
					{
						File.Move(text + ".new", text);
					}
					this._lastSavedCounter = count;
				}
			}
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001A77DC File Offset: 0x001A59DC
		public AssetEditorSettings Clone()
		{
			return new AssetEditorSettings
			{
				FormatVersion = this.FormatVersion,
				UncollapsedDirectories = new List<string>(this.UncollapsedDirectories),
				ActiveAssetTree = this.ActiveAssetTree,
				AssetsPath = this.AssetsPath,
				DisplayDefaultAssetPathWarning = this.DisplayDefaultAssetPathWarning,
				LastUsedVersion = this.LastUsedVersion,
				DiagnosticMode = this.DiagnosticMode,
				Language = this.Language,
				Maximized = this.Maximized,
				UseBorderlessForFullscreen = this.UseBorderlessForFullscreen,
				Fullscreen = this.Fullscreen
			};
		}

		// Token: 0x040034FE RID: 13566
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040034FF RID: 13567
		public const int CurrentVersion = 1;

		// Token: 0x04003500 RID: 13568
		public int FormatVersion;

		// Token: 0x04003501 RID: 13569
		public bool Fullscreen;

		// Token: 0x04003502 RID: 13570
		public bool UseBorderlessForFullscreen;

		// Token: 0x04003503 RID: 13571
		public bool Maximized;

		// Token: 0x04003504 RID: 13572
		public string Language;

		// Token: 0x04003505 RID: 13573
		public bool DiagnosticMode = true;

		// Token: 0x04003506 RID: 13574
		public List<string> UncollapsedDirectories = new List<string>();

		// Token: 0x04003507 RID: 13575
		public AssetTreeFolder ActiveAssetTree = AssetTreeFolder.Server;

		// Token: 0x04003508 RID: 13576
		public string AssetsPath;

		// Token: 0x04003509 RID: 13577
		public bool DisplayDefaultAssetPathWarning;

		// Token: 0x0400350A RID: 13578
		public string LastUsedVersion;

		// Token: 0x0400350B RID: 13579
		public readonly Dictionary<AssetEditorSettings.Panes, int> PaneSizes = new Dictionary<AssetEditorSettings.Panes, int>
		{
			{
				AssetEditorSettings.Panes.AssetBrowser,
				300
			},
			{
				AssetEditorSettings.Panes.ConfigEditorSidebar,
				280
			},
			{
				AssetEditorSettings.Panes.ConfigEditorSidebarPreviewModel,
				380
			},
			{
				AssetEditorSettings.Panes.ConfigEditorSidebarPreviewItem,
				280
			},
			{
				AssetEditorSettings.Panes.Diagnostics,
				250
			},
			{
				AssetEditorSettings.Panes.ConfigEditorPropertyNames,
				250
			}
		};

		// Token: 0x0400350C RID: 13580
		private readonly object _saveLock = new object();

		// Token: 0x0400350D RID: 13581
		private int _lastSavedCounter;

		// Token: 0x0400350E RID: 13582
		private int _saveCounter;

		// Token: 0x02000F15 RID: 3861
		public enum Panes
		{
			// Token: 0x04004A01 RID: 18945
			AssetBrowser,
			// Token: 0x04004A02 RID: 18946
			ConfigEditorSidebar,
			// Token: 0x04004A03 RID: 18947
			ConfigEditorSidebarPreviewModel,
			// Token: 0x04004A04 RID: 18948
			ConfigEditorSidebarPreviewItem,
			// Token: 0x04004A05 RID: 18949
			Diagnostics,
			// Token: 0x04004A06 RID: 18950
			ConfigEditorPropertyNames
		}
	}
}
