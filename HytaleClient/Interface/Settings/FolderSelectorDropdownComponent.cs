using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Utils;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080B RID: 2059
	internal class FolderSelectorDropdownComponent : SettingComponent<string>
	{
		// Token: 0x06003915 RID: 14613 RVA: 0x00077308 File Offset: 0x00075508
		public FolderSelectorDropdownComponent(Desktop desktop, Group parent, string name, ISettingView settings) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("FileSelectorDropdownSetting.ui", out document);
			Group parent2 = uifragment.Get<Group>("FileSelectorContainer");
			UIPath uipath = document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "FileDropdownTemplate");
			this._fileDropdownBox = new FileDropdownBox(this.Desktop, parent2, uipath.Value, () => FolderSelectorDropdownComponent.GetFiles(this._fileDropdownBox.CurrentPath));
			this._fileDropdownBox.Style = document.ResolveNamedValue<FileDropdownBoxStyle>(this.Desktop.Provider, "FileDropdownBoxStyle");
			this._fileDropdownBox.AllowDirectorySelection = true;
			this._fileDropdownBox.IsSearchEnabled = false;
			this._fileDropdownBox.ValueChanged = delegate()
			{
				HashSet<string> selectedFiles = this._fileDropdownBox.SelectedFiles;
				string text = FolderSelectorDropdownComponent.NormalizeDropdownSelection((selectedFiles != null) ? Enumerable.FirstOrDefault<string>(selectedFiles) : null);
				this.SetValue(text);
				this.OnChange(text);
			};
			this._fileDropdownBox.DropdownToggled = delegate()
			{
				bool flag = !this._fileDropdownBox.IsOpen;
				if (!flag)
				{
					HashSet<string> selectedFiles = this._fileDropdownBox.SelectedFiles;
					string file = (selectedFiles != null) ? Enumerable.FirstOrDefault<string>(selectedFiles) : null;
					string text = FolderSelectorDropdownComponent.NormalizePath(file);
					this._fileDropdownBox.Setup(text, FolderSelectorDropdownComponent.GetFiles(text));
				}
			};
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x000773E0 File Offset: 0x000755E0
		public override void SetValue(string value)
		{
			value = UnixPathUtil.ConvertToUnixPath(value);
			FileDropdownBox fileDropdownBox = this._fileDropdownBox;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add((value != null) ? value.Trim(new char[]
			{
				'/'
			}) : null);
			fileDropdownBox.SelectedFiles = hashSet;
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x00077428 File Offset: 0x00075628
		private static string NormalizeDropdownSelection(string path)
		{
			bool flag = BuildInfo.Platform == Platform.Windows && path == string.Empty;
			if (flag)
			{
				path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			else
			{
				bool flag2 = BuildInfo.Platform != Platform.Windows && path != null;
				if (flag2)
				{
					path = "/" + path;
				}
			}
			return path;
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x00077484 File Offset: 0x00075684
		private static string NormalizePath(string file)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			bool flag = BuildInfo.Platform == Platform.Windows;
			if (flag)
			{
				bool flag2 = file != null && Directory.Exists(file);
				if (flag2)
				{
					string directoryName = Path.GetDirectoryName(file);
					text = (((directoryName != null) ? directoryName.TrimEnd(new char[]
					{
						Path.DirectorySeparatorChar
					}) : null) ?? "");
				}
				text = UnixPathUtil.ConvertToUnixPath(text);
			}
			else
			{
				bool flag3 = file != null && Directory.Exists("/" + file);
				if (flag3)
				{
					text = (Path.GetDirectoryName("/" + file) ?? '/'.ToString());
				}
			}
			return text;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x00077534 File Offset: 0x00075734
		private static List<FileSelector.File> GetFiles(string path)
		{
			bool flag = BuildInfo.Platform == Platform.Windows;
			if (flag)
			{
				path = path.TrimStart(new char[]
				{
					'/'
				}) + "/";
			}
			List<FileSelector.File> list = new List<FileSelector.File>();
			try
			{
				bool flag2 = BuildInfo.Platform == Platform.Windows && path == '/'.ToString();
				if (flag2)
				{
					foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
					{
						list.Add(new FileSelector.File
						{
							Name = driveInfo.Name.TrimEnd(new char[]
							{
								'\\'
							}),
							IsDirectory = true
						});
					}
				}
				else
				{
					path = Path.GetFullPath(path);
					foreach (string text in Directory.GetDirectories(path))
					{
						bool flag3 = (File.GetAttributes(text) & FileAttributes.Hidden) == FileAttributes.Hidden;
						bool flag4 = flag3;
						if (!flag4)
						{
							list.Add(new FileSelector.File
							{
								Name = Path.GetFileName(text),
								IsDirectory = true
							});
						}
					}
				}
			}
			catch (IOException exception)
			{
				Interface.Logger.Error(exception, "Failed to fetch files for component in {0}", new object[]
				{
					path
				});
			}
			list.Sort((FileSelector.File a, FileSelector.File b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			return list;
		}

		// Token: 0x040018C4 RID: 6340
		private readonly FileDropdownBox _fileDropdownBox;
	}
}
