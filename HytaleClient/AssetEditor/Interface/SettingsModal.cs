using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.Data.UserSettings;
using HytaleClient.Interface.Settings;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor.Interface
{
	// Token: 0x02000B97 RID: 2967
	internal class SettingsModal : BaseModal, ISettingView
	{
		// Token: 0x06005B9E RID: 23454 RVA: 0x001CAB47 File Offset: 0x001C8D47
		public SettingsModal(AssetEditorInterface @interface) : base(@interface, "Settings/SettingsModal.ui")
		{
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x001CAB57 File Offset: 0x001C8D57
		protected override void BuildModal(Document doc, UIFragment fragment)
		{
			this.BuildSettings();
			this.ApplySettings();
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x001CAB68 File Offset: 0x001C8D68
		private void BuildSettings()
		{
			this._content.Clear();
			AssetEditorApp app = this._interface.App;
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>
			{
				new DropdownBox.DropdownEntryInfo(this.Desktop.Provider.GetText("ui.assetEditor.settings.setting.useSystemLanguage", null, true), "", false)
			};
			foreach (KeyValuePair<string, string> keyValuePair in Language.GetAvailableLanguages())
			{
				list.Add(new DropdownBox.DropdownEntryInfo(keyValuePair.Value, keyValuePair.Key, false));
			}
			this._languageSetting = this.AddDropdownSetting(this._content, "ui.assetEditor.settings.setting.language", list, delegate(string value)
			{
				AssetEditorSettings assetEditorSettings = app.Settings.Clone();
				assetEditorSettings.Language = ((value == "") ? null : value);
				app.ApplySettings(assetEditorSettings);
			});
			this._fullscreenSetting = this.AddDropdownSetting<SettingsModal.WindowMode>(this._content, "ui.assetEditor.settings.setting.fullscreen", new List<KeyValuePair<string, SettingsModal.WindowMode>>
			{
				new KeyValuePair<string, SettingsModal.WindowMode>(this.Desktop.Provider.GetText("ui.assetEditor.settings.setting.fullscreen", null, true), SettingsModal.WindowMode.Fullscreen),
				new KeyValuePair<string, SettingsModal.WindowMode>(this.Desktop.Provider.GetText("ui.assetEditor.settings.setting.borderlessFullscreen", null, true), SettingsModal.WindowMode.WindowedFullscreen),
				new KeyValuePair<string, SettingsModal.WindowMode>(this.Desktop.Provider.GetText("ui.assetEditor.settings.setting.windowed", null, true), SettingsModal.WindowMode.Window)
			}, delegate(SettingsModal.WindowMode value)
			{
				AssetEditorSettings assetEditorSettings = app.Settings.Clone();
				assetEditorSettings.Fullscreen = (value > SettingsModal.WindowMode.Window);
				assetEditorSettings.UseBorderlessForFullscreen = (value == SettingsModal.WindowMode.WindowedFullscreen);
				app.ApplySettings(assetEditorSettings);
			});
			this._diagnosticsModeSetting = this.AddCheckBoxSetting(this._content, "ui.assetEditor.settings.setting.diagnosticMode", delegate(bool value)
			{
				AssetEditorSettings assetEditorSettings = app.Settings.Clone();
				assetEditorSettings.DiagnosticMode = value;
				app.ApplySettings(assetEditorSettings);
			});
			this._assetsPathSetting = this.AddFolderSelectorDropdownSetting(this._content, "ui.assetEditor.settings.setting.assetsPath", delegate(string value)
			{
				bool flag = value == app.Settings.AssetsPath;
				if (!flag)
				{
					AssetEditorSettings assetEditorSettings = app.Settings.Clone();
					assetEditorSettings.AssetsPath = value;
					assetEditorSettings.DisplayDefaultAssetPathWarning = false;
					app.ApplySettings(assetEditorSettings);
				}
			});
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x001CAD24 File Offset: 0x001C8F24
		private void ApplySettings()
		{
			AssetEditorSettings settings = this._interface.App.Settings;
			this._languageSetting.SetValue(settings.Language ?? "");
			this._fullscreenSetting.SetValue(settings.Fullscreen ? (settings.UseBorderlessForFullscreen ? SettingsModal.WindowMode.WindowedFullscreen : SettingsModal.WindowMode.Fullscreen).ToString() : SettingsModal.WindowMode.Window.ToString());
			this._diagnosticsModeSetting.SetValue(settings.DiagnosticMode);
			this._assetsPathSetting.SetValue(settings.AssetsPath);
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x001CADC4 File Offset: 0x001C8FC4
		private CheckBoxSettingComponent AddCheckBoxSetting(Group container, string name, Action<bool> onChange)
		{
			return new CheckBoxSettingComponent(this.Desktop, container, name, this)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x001CADEC File Offset: 0x001C8FEC
		private SliderSettingComponent AddSliderSetting(Group container, string name, int min, int max, int step, Action<int> onChange)
		{
			return new SliderSettingComponent(this.Desktop, container, name, this, min, max, step)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x001CAE1C File Offset: 0x001C901C
		private DropdownSettingComponent AddDropdownSetting<T>(Group container, string name, List<KeyValuePair<string, T>> values, Action<T> onChange) where T : struct, IConvertible
		{
			return new DropdownSettingComponent(this.Desktop, container, name, this, Enumerable.ToList<DropdownBox.DropdownEntryInfo>(Enumerable.Select<KeyValuePair<string, T>, DropdownBox.DropdownEntryInfo>(values, delegate(KeyValuePair<string, T> e)
			{
				string key = e.Key;
				T value = e.Value;
				return new DropdownBox.DropdownEntryInfo(key, value.ToString(), false);
			})))
			{
				OnChange = delegate(string v)
				{
					onChange((T)((object)Enum.Parse(typeof(T), v)));
				}
			};
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x001CAE88 File Offset: 0x001C9088
		private DropdownSettingComponent AddDropdownSetting(Group container, string name, List<DropdownBox.DropdownEntryInfo> values, Action<string> onChange)
		{
			return new DropdownSettingComponent(this.Desktop, container, name, this, values)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x001CAEB4 File Offset: 0x001C90B4
		private FolderSelectorDropdownComponent AddFolderSelectorDropdownSetting(Group container, string name, Action<string> onChange)
		{
			return new FolderSelectorDropdownComponent(this.Desktop, container, name, this)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x001CAEDB File Offset: 0x001C90DB
		protected internal override void Validate()
		{
			this.Dismiss();
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x001CAEE4 File Offset: 0x001C90E4
		public void SetHoveredSetting<T>(string setting, SettingComponent<T> component)
		{
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x001CAEE8 File Offset: 0x001C90E8
		public bool TryGetDocument(string path, out Document document)
		{
			return this.Desktop.Provider.TryGetDocument("Settings/" + path, out document);
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x001CAF16 File Offset: 0x001C9116
		public void Open()
		{
			base.OpenInLayer();
		}

		// Token: 0x0400394B RID: 14667
		private DropdownSettingComponent _languageSetting;

		// Token: 0x0400394C RID: 14668
		private DropdownSettingComponent _fullscreenSetting;

		// Token: 0x0400394D RID: 14669
		private CheckBoxSettingComponent _diagnosticsModeSetting;

		// Token: 0x0400394E RID: 14670
		private FolderSelectorDropdownComponent _assetsPathSetting;

		// Token: 0x02000F97 RID: 3991
		private enum WindowMode
		{
			// Token: 0x04004B74 RID: 19316
			Window,
			// Token: 0x04004B75 RID: 19317
			Fullscreen,
			// Token: 0x04004B76 RID: 19318
			WindowedFullscreen
		}
	}
}
