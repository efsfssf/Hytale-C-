using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HytaleClient.Application;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Interface.Settings.Options;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x02000810 RID: 2064
	internal class SettingsComponent : InterfaceComponent, ISettingView
	{
		// Token: 0x0600392B RID: 14635 RVA: 0x000779B0 File Offset: 0x00075BB0
		public SettingsComponent(Interface @interface, Group parent) : base(@interface, parent)
		{
			this._easingTypes = new List<DropdownBox.DropdownEntryInfo>();
			foreach (string text in Enum.GetNames(typeof(Easing.EasingType)))
			{
				this._easingTypes.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
			}
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00077A34 File Offset: 0x00075C34
		public void Build()
		{
			SettingsComponent.<>c__DisplayClass116_0 CS$<>8__locals1 = new SettingsComponent.<>c__DisplayClass116_0();
			CS$<>8__locals1.<>4__this = this;
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("Common/Settings/Settings.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			CS$<>8__locals1.navigationGroup = uifragment.Get<Group>("Navigation");
			this._buttonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "NavigationButtonStyle");
			this._buttonSelectedStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "NavigationButtonSelectedStyle");
			this._settingInfo = uifragment.Get<Group>("SettingInfo");
			this._settingInfoLabel = uifragment.Get<Label>("SettingInfoLabel");
			this._tabButtons.Clear();
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.General, "ui.settings.tabs.general");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Visual, "ui.settings.tabs.visual");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Audio, "ui.settings.tabs.audio");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Mouse, "ui.settings.tabs.mouse");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Controls, "ui.settings.tabs.controls");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Creative, "ui.settings.tabs.creative");
			CS$<>8__locals1.<Build>g__AddTabButton|0(SettingsComponent.SettingsTab.Prototype, "ui.settings.tabs.prototype");
			this._tabs.Clear();
			CS$<>8__locals1.container = (this._tabContainer = uifragment.Get<Group>("TabContainer"));
			CS$<>8__locals1.app = this.Interface.App;
			Dictionary<SettingsComponent.SettingsTab, Group> tabs = this._tabs;
			SettingsComponent.SettingsTab key = SettingsComponent.SettingsTab.General;
			Group group = new Group(this.Desktop, CS$<>8__locals1.container);
			group.LayoutMode = LayoutMode.Top;
			Group group2 = group;
			tabs[key] = group;
			Group container = group2;
			this.AddSectionHeader(container, "ui.settings.groups.general");
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>
			{
				new DropdownBox.DropdownEntryInfo(this.Desktop.Provider.GetText("ui.settings.useSystemLanguage", null, true), "", false)
			};
			foreach (KeyValuePair<string, string> keyValuePair in Language.GetAvailableLanguages())
			{
				list.Add(new DropdownBox.DropdownEntryInfo(keyValuePair.Value, keyValuePair.Key, false));
			}
			this._languageSetting = this.AddDropdownSetting(container, "ui.settings.language", list, delegate(string value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.Language = ((value == "") ? null : value);
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._fullscreenSetting = this.AddDropdownSetting<SettingsComponent.WindowMode>(container, "ui.settings.fullscreen", new List<KeyValuePair<string, SettingsComponent.WindowMode>>
			{
				new KeyValuePair<string, SettingsComponent.WindowMode>(this.Desktop.Provider.GetText("ui.settings.fullscreen", null, true), SettingsComponent.WindowMode.Fullscreen),
				new KeyValuePair<string, SettingsComponent.WindowMode>(this.Desktop.Provider.GetText("ui.settings.borderlessFullscreen", null, true), SettingsComponent.WindowMode.WindowedFullscreen),
				new KeyValuePair<string, SettingsComponent.WindowMode>(this.Desktop.Provider.GetText("ui.settings.windowed", null, true), SettingsComponent.WindowMode.Window)
			}, delegate(SettingsComponent.WindowMode value)
			{
				bool fullscreen = CS$<>8__locals1.app.Settings.Fullscreen;
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.Fullscreen = (value > SettingsComponent.WindowMode.Window);
				settings.UseBorderlessForFullscreen = (value == SettingsComponent.WindowMode.WindowedFullscreen);
				bool flag2 = fullscreen && !settings.Fullscreen;
				if (flag2)
				{
					IReadOnlyList<DropdownBox.DropdownEntryInfo> entries = CS$<>8__locals1.<>4__this._resolutionSetting.Dropdown.Entries;
					DropdownBox.DropdownEntryInfo dropdownEntryInfo = entries[entries.Count - 3];
					CS$<>8__locals1.<>4__this._resolutionSetting.SetValue(dropdownEntryInfo.Value);
					settings.ScreenResolution = ScreenResolution.FromString(dropdownEntryInfo.Value);
				}
				else
				{
					bool flag3 = !fullscreen && settings.Fullscreen;
					if (flag3)
					{
						IReadOnlyList<DropdownBox.DropdownEntryInfo> entries2 = CS$<>8__locals1.<>4__this._resolutionSetting.Dropdown.Entries;
						DropdownBox.DropdownEntryInfo dropdownEntryInfo2 = entries2[entries2.Count - 2];
						CS$<>8__locals1.<>4__this._resolutionSetting.SetValue(dropdownEntryInfo2.Value);
						settings.ScreenResolution = ScreenResolution.FromString(dropdownEntryInfo2.Value);
					}
				}
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.AddSectionSpacer(container);
			this.AddSectionHeader(container, "ui.settings.groups.ui");
			this._maxChatMessagesSetting = this.AddSliderSetting(container, "ui.settings.maxChatMessages", 16, 256, 8, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MaxChatMessages = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.AddSectionSpacer(container);
			this.AddSectionHeader(container, "ui.settings.groups.development");
			this._diagnosticsModeSetting = this.AddCheckBoxSetting(container, "ui.settings.diagnosticMode", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.DiagnosticMode = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this._inputBindingSettings["OpenDevTools"].Visible = CS$<>8__locals1.<>4__this.Interface.App.Settings.DiagnosticMode;
			});
			Dictionary<SettingsComponent.SettingsTab, Group> tabs2 = this._tabs;
			SettingsComponent.SettingsTab key2 = SettingsComponent.SettingsTab.Visual;
			Group group3 = new Group(this.Desktop, CS$<>8__locals1.container);
			group3.LayoutMode = LayoutMode.Top;
			group2 = group3;
			tabs2[key2] = group3;
			Group container2 = group2;
			this.AddSectionHeader(container2, "ui.settings.groups.rendering");
			List<KeyValuePair<string, string>> availableResolutionOptions = ScreenResolutions.GetAvailableResolutionOptions(CS$<>8__locals1.app);
			availableResolutionOptions.Add(new KeyValuePair<string, string>(this.Desktop.Provider.GetText("ui.settings.customResolution", null, true), ScreenResolutions.CustomScreenResolution.ToString()));
			this._resolutionSetting = this.AddDropdownSetting(container2, "ui.settings.resolution", availableResolutionOptions, delegate(string value)
			{
				ScreenResolution screenResolution = ScreenResolution.FromString(value);
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.ScreenResolution = screenResolution;
				settings.Fullscreen = screenResolution.Fullscreen;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this._fullscreenSetting.SetValue(screenResolution.Fullscreen ? (settings.UseBorderlessForFullscreen ? SettingsComponent.WindowMode.WindowedFullscreen : SettingsComponent.WindowMode.Fullscreen).ToString() : SettingsComponent.WindowMode.Window.ToString());
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._vsyncSetting = this.AddCheckBoxSetting(container2, "ui.settings.vsync", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.VSync = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._unlimitedFpsSetting = this.AddCheckBoxSetting(container2, "ui.settings.unlimitedFps", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.UnlimitedFps = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this._maxFpsSetting.Visible = !value;
				CS$<>8__locals1.container.Layout(null, true);
			});
			this._maxFpsSetting = this.AddSliderSetting(container2, "ui.settings.fpsLimit", 20, 240, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.FpsLimit = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._viewDistanceSetting = this.AddSliderSetting(container2, "ui.settings.viewDistance", 32, 1024, 32, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.ViewDistance = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._fieldOfViewSetting = this.AddSliderSetting(container2, "ui.settings.fieldOfView", 30, 120, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.FieldOfView = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._automaticRenderScaleSetting = this.AddCheckBoxSetting(container2, "ui.settings.automaticRenderScale", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AutomaticRenderScale = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this._renderScaleSetting.Visible = !value;
				CS$<>8__locals1.container.Layout(null, true);
			});
			this._renderScaleSetting = this.AddSliderSetting(container2, "ui.settings.renderScale", 50, 200, 5, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.RenderScale = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._placementPreviewSetting = this.AddDropdownSetting<BlockPlacementPreview.DisplayMode>(container2, "ui.settings.placementPreview", new List<KeyValuePair<string, BlockPlacementPreview.DisplayMode>>
			{
				new KeyValuePair<string, BlockPlacementPreview.DisplayMode>(this.Desktop.Provider.GetText("ui.settings.placementPreview.none", null, true), BlockPlacementPreview.DisplayMode.None),
				new KeyValuePair<string, BlockPlacementPreview.DisplayMode>(this.Desktop.Provider.GetText("ui.settings.placementPreview.all", null, true), BlockPlacementPreview.DisplayMode.All),
				new KeyValuePair<string, BlockPlacementPreview.DisplayMode>(this.Desktop.Provider.GetText("ui.settings.placementPreview.multipart", null, true), BlockPlacementPreview.DisplayMode.Multipart)
			}, delegate(BlockPlacementPreview.DisplayMode value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.PlacementPreviewMode = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			Dictionary<SettingsComponent.SettingsTab, Group> tabs3 = this._tabs;
			SettingsComponent.SettingsTab key3 = SettingsComponent.SettingsTab.Mouse;
			Group group4 = new Group(this.Desktop, CS$<>8__locals1.container);
			group4.LayoutMode = LayoutMode.Top;
			group2 = group4;
			tabs3[key3] = group4;
			Group container3 = group2;
			this.AddSectionHeader(container3, "ui.settings.groups.mouse");
			this._mouseRawInputModeSetting = this.AddCheckBoxSetting(container3, "ui.settings.mouseRawInputMode", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MouseSettings.MouseRawInputMode = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._mouseInvertedSetting = this.AddCheckBoxSetting(container3, "ui.settings.mouseInverted", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MouseSettings.MouseInverted = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._mouseXSpeedSetting = this.AddFloatSliderSetting(container3, "ui.settings.mouseXSpeed", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MouseSettings.MouseXSpeed = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._mouseYSpeedSetting = this.AddFloatSliderSetting(container3, "ui.settings.mouseYSpeed", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MouseSettings.MouseYSpeed = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			Dictionary<SettingsComponent.SettingsTab, Group> tabs4 = this._tabs;
			SettingsComponent.SettingsTab key4 = SettingsComponent.SettingsTab.Audio;
			Group group5 = new Group(this.Desktop, CS$<>8__locals1.container);
			group5.LayoutMode = LayoutMode.Top;
			group2 = group5;
			tabs4[key4] = group5;
			Group container4 = group2;
			this.AddSectionHeader(container4, "ui.settings.groups.audioOutput");
			this._outputDeviceSetting = this.AddDropdownSetting(container4, "ui.settings.outputDevice", new List<DropdownBox.DropdownEntryInfo>(), delegate(string value)
			{
				uint outputDeviceId;
				bool flag2 = !uint.TryParse(value, out outputDeviceId);
				if (flag2)
				{
					outputDeviceId = 0U;
				}
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.OutputDeviceId = outputDeviceId;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.UpdateAudioOutputDeviceList();
			this._masterVolumeSetting = this.AddSliderSetting(container4, "ui.settings.masterVolume", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.MasterVolume = (float)value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._musicVolumeSetting = this.AddSliderSetting(container4, "ui.settings.musicVolume", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.CategoryVolumes["MusicVolume"] = (float)value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._ambientVolumeSetting = this.AddSliderSetting(container4, "ui.settings.ambientVolume", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.CategoryVolumes["AmbienceVolume"] = (float)value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._uiVolumeSetting = this.AddSliderSetting(container4, "ui.settings.uiVolume", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.CategoryVolumes["UIVolume"] = (float)value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._sfxVolumeSetting = this.AddSliderSetting(container4, "ui.settings.sfxVolume", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.AudioSettings.CategoryVolumes["SFXVolume"] = (float)value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			Dictionary<SettingsComponent.SettingsTab, Group> tabs5 = this._tabs;
			SettingsComponent.SettingsTab key5 = SettingsComponent.SettingsTab.Controls;
			Group group6 = new Group(this.Desktop, CS$<>8__locals1.container);
			group6.LayoutMode = LayoutMode.Top;
			group2 = group6;
			tabs5[key5] = group6;
			Group group7 = group2;
			this.AddSectionHeader(group7, "ui.settings.groups.controls");
			this._inputBindingSettings.Clear();
			foreach (FieldInfo fieldInfo in typeof(InputBindings).GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				bool flag = fieldInfo.FieldType == typeof(InputBinding);
				if (flag)
				{
					this._inputBindingSettings[fieldInfo.Name] = this.AddInputBinding(group7, fieldInfo.Name);
				}
			}
			group7.Children[group7.Children.Count - 1].Children[0].Anchor.Bottom = new int?(0);
			Dictionary<SettingsComponent.SettingsTab, Group> tabs6 = this._tabs;
			SettingsComponent.SettingsTab key6 = SettingsComponent.SettingsTab.Creative;
			Group group8 = new Group(this.Desktop, CS$<>8__locals1.container);
			group8.LayoutMode = LayoutMode.Top;
			group2 = group8;
			tabs6[key6] = group8;
			Group container5 = group2;
			this.AddSectionHeader(container5, "ui.settings.groups.builderTools");
			this._showBuilderToolsNotificationsSetting = this.AddCheckBoxSetting(container5, "ui.settings.showToolNotifications", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.ShowBuilderToolsNotifications = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolEnableBrushSpacing = this.AddCheckBoxSetting(container5, "ui.settings.enableBrushSpacing", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.EnableBrushSpacing = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsSpacingBlocksOffset = this.AddSliderSetting(container5, "ui.settings.brushSpacingBlocks", 0, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BrushSpacingBlocks = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsUseToolReachDistanceSetting = this.AddCheckBoxSetting(container5, "ui.settings.useToolReachDistance", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.useToolReachDistance = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this.GatherSettings();
			});
			this._builderToolsToolReachDistanceSetting = this.AddSliderSetting(container5, "ui.settings.toolReachDistance", 1, 256, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.ToolReachDistance = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsToolReachLockSetting = this.AddCheckBoxSetting(container5, "ui.settings.toolReachLock", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.ToolReachLock = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsToolDelayMinSetting = this.AddSliderSetting(container5, "ui.settings.toolDelayMin", 1, 1000, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.ToolDelayMin = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsBrushOpacitySetting = this.AddSliderSetting(container5, "ui.settings.brushOpacity", 1, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.BrushOpacity = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsSelectionOpacitySetting = this.AddSliderSetting(container5, "ui.settings.selectionOpacity", 1, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.SelectionOpacity = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsDisplayLegendSetting = this.AddCheckBoxSetting(container5, "ui.settings.displayLegend", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.DisplayLegend = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			Dictionary<SettingsComponent.SettingsTab, Group> tabs7 = this._tabs;
			SettingsComponent.SettingsTab key7 = SettingsComponent.SettingsTab.Prototype;
			Group group9 = new Group(this.Desktop, CS$<>8__locals1.container);
			group9.LayoutMode = LayoutMode.Top;
			group2 = group9;
			tabs7[key7] = group9;
			Group group10 = group2;
			this.AddSectionHeader(group10, "ui.settings.groups.general");
			this._builderModeSetting = this.AddCheckBoxSetting(group10, "ui.settings.builderMode", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderMode = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderModeSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.builderMode.tooltip", null, true);
			this._blockHealthSetting = this.AddCheckBoxSetting(group10, "ui.settings.blockHealth", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BlockHealth = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._blockHealthSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.blockHealth.tooltip", null, true);
			this._creativeInteractionDistance = this.AddSliderSetting(group10, "ui.settings.creativeInteractionDistance", 1, 128, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.creativeInteractionDistance = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._adventureInteractionDistance = this.AddSliderSetting(group10, "ui.settings.adventureInteractionDistance", 1, 128, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.adventureInteractionDistance = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._builderToolsEnableBrushShapeRenderingSetting = this.AddCheckBoxSetting(group10, "ui.settings.enableBrushShapeRendering", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BuilderToolsSettings.EnableBrushShapeRendering = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._placeBlocksAtRange = this.AddCheckBoxSetting(group10, "ui.settings.placeBlocksAtRange", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings._placeBlocksAtRange = value;
				bool flag2 = !value;
				if (flag2)
				{
					settings.PlaceBlocksAtRangeInAdventureMode = false;
				}
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this.GatherSettings();
			});
			this._placeBlocksAtRangeInAdventureMode = this.AddCheckBoxSetting(group10, "ui.settings.placeBlocksAtRangeInAdventureMode", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.PlaceBlocksAtRangeInAdventureMode = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._useBlockSubfaces = this.AddCheckBoxSetting(group10, "ui.settings.useBlockSubfaces", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.UseBlockSubfaces = value;
				settings.DisplayBlockSubfaces = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
				CS$<>8__locals1.<>4__this.GatherSettings();
			});
			this._displayBlockSubfaces = this.AddCheckBoxSetting(group10, "ui.settings.displayBlockSubfaces", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.DisplayBlockSubfaces = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._displayBlockBoundaries = this.AddCheckBoxSetting(group10, "ui.settings.displayBlockBoundaries", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.DisplayBlockBoundaries = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._blockPlacementSupportValidation = this.AddCheckBoxSetting(group10, "ui.settings.blockPlacementSupportValidation", delegate(bool value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.BlockPlacementSupportValidation = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._resetMouseSensitivityDuration = this.AddFloatSliderSetting(group10, "ui.settings.resetMouseSensitivityDuration", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.ResetMouseSensitivityDuration = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.AddSectionSpacer(group10);
			this.AddSectionHeader(group10, "ui.settings.groups.playselect");
			this._percentageOfPlaySelectionLengthGizmoShouldRenderSetting = this.AddSliderSetting(group10, "ui.settings.percentagePlayGizmoLength", 1, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.PercentageOfPlaySelectionLengthGizmoShouldRender = (float)value / 100f;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._minPlaySelectGizmoSizeSetting = this.AddSliderSetting(group10, "ui.settings.minPlaySelectGizmoSize", 1, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MinPlaySelectGizmoSize = MathHelper.Clamp(value, 1, settings.MaxPlaySelectGizmoSize - 1);
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this._maxPlaySelectGizmoSizeSetting = this.AddSliderSetting(group10, "ui.settings.maxPlaySelectGizmoSize", 2, 100, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.MaxPlaySelectGizmoSize = MathHelper.Clamp(value, settings.MinPlaySelectGizmoSize + 1, value);
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.AddSectionSpacer(group10);
			this.AddSectionHeader(group10, "ui.settings.groups.playpaint");
			this._builderToolsPaintOperationsIgnoreHistoryLengthSetting = this.AddSliderSetting(group10, "ui.settings.paintOperationIgnoreHistoryLength", 0, 200, 1, delegate(int value)
			{
				Settings settings = CS$<>8__locals1.app.Settings.Clone();
				settings.PaintOperationsIgnoreHistoryLength = value;
				CS$<>8__locals1.app.ApplyNewSettings(settings);
			});
			this.AddMovementSettings(group10, CS$<>8__locals1.app);
			this.AddMantlingSettings(group10, CS$<>8__locals1.app);
			this.AddSprintForceSettings(group10, CS$<>8__locals1.app);
			this.AddSlideForceSettings(group10, CS$<>8__locals1.app);
			this.AddCameraSettings(group10, CS$<>8__locals1.app);
			this.AddStaminaSettings(group10, CS$<>8__locals1.app);
			this.AddWeaponPullbackSettings(group10, CS$<>8__locals1.app);
			this.AddEntityUISettings(group10, CS$<>8__locals1.app);
			this.AddDebugSettings(group10, CS$<>8__locals1.app);
			this.AddMountSettings(group10, CS$<>8__locals1.app);
			this._inputBindingPopup = new InputBindingPopup(this);
			uifragment.Get<TextButton>("ResetSettings").Activating = delegate()
			{
				Settings newSettings = Settings.MakeDefaults();
				CS$<>8__locals1.app.ApplyNewSettings(newSettings);
				CS$<>8__locals1.<>4__this.GatherSettings();
			};
			uifragment.Get<TextButton>("SaveChanges").Activating = delegate()
			{
				CS$<>8__locals1.<>4__this.SaveChanges();
				CS$<>8__locals1.<>4__this.Dismiss();
			};
			this.UpdateGroup();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.GatherSettings();
			}
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x000787BC File Offset: 0x000769BC
		private void SaveChanges()
		{
			this.Interface.App.Settings.Save();
			AppInGame inGame = this.Interface.InGameView.InGame;
			ConnectionToServer connectionToServer;
			if (inGame == null)
			{
				connectionToServer = null;
			}
			else
			{
				GameInstance instance = inGame.Instance;
				connectionToServer = ((instance != null) ? instance.Connection : null);
			}
			ConnectionToServer connectionToServer2 = connectionToServer;
			if (connectionToServer2 != null)
			{
				connectionToServer2.SendPacket(new SyncPlayerPreferences(this.Interface.App.Settings.DebugSettings.ShowDebugMarkers));
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x00078834 File Offset: 0x00076A34
		private void AddDebugSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.debug");
			this._showDebugMarkersSetting = this.AddCheckBoxSetting(group, "ui.settings.showDebugMarkers", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.DebugSettings.ShowDebugMarkers = value;
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x00078884 File Offset: 0x00076A84
		private void AddMovementSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.movement");
			this._autoJumpObstacleSetting = this.AddCheckBoxSetting(group, "ui.settings.autoJumpObstacle", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.AutoJumpObstacle = value;
				app.ApplyNewSettings(settings);
			});
			this._autoJumpGap = this.AddCheckBoxSetting(group, "ui.settings.autoJumpGap", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.AutoJumpGap = value;
				app.ApplyNewSettings(settings);
			});
			this._maxJumpForceSpeedMultiplier = this.AddFloatSliderSetting(group, "ui.settings.maxJumpForceSpeedMultiplier", 0f, 100f, 1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MaxJumpForceSpeedMultiplier = value;
				app.ApplyNewSettings(settings);
			});
			this._jumpForceSpeedMultiplierStep = this.AddFloatSliderSetting(group, "ui.settings.jumpForceSpeedMultiplierStep", 0f, 10f, 1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.JumpForceSpeedMultiplierStep = value;
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0007894C File Offset: 0x00076B4C
		private void AddMantlingSettings(Group group, App app)
		{
			bool flag = app.InGame.Instance != null && !app.InGame.Instance.ClientFeatureModule.IsFeatureEnabled(2);
			if (!flag)
			{
				this.AddSectionSpacer(group);
				this.AddSectionHeader(group, "ui.settings.groups.mantling");
				this._mantlingSetting = this.AddCheckBoxSetting(group, "ui.settings.mantling", delegate(bool value)
				{
					Settings settings = app.Settings.Clone();
					settings.Mantling = value;
					app.ApplyNewSettings(settings);
				});
				this._minVelocityMantlingSetting = this.AddFloatSliderSetting(group, "ui.settings.minVelocityMantling", -200f, 0f, 0.1f, delegate(float value)
				{
					Settings settings = app.Settings.Clone();
					settings.MinVelocityMantling = value;
					app.ApplyNewSettings(settings);
				});
				this._maxVelocityMantlingSetting = this.AddFloatSliderSetting(group, "ui.settings.maxVelocityMantling", 0f, 200f, 0.1f, delegate(float value)
				{
					Settings settings = app.Settings.Clone();
					settings.MaxVelocityMantling = value;
					app.ApplyNewSettings(settings);
				});
				this._mantlingCameraOffsetY = this.AddFloatSliderSetting(group, "ui.settings.mantlingCameraOffsetY", -5f, 0f, 0.1f, delegate(float value)
				{
					Settings settings = app.Settings.Clone();
					settings.MantlingCameraOffsetY = value;
					app.ApplyNewSettings(settings);
				});
				this._mantleBlockHeight = this.AddFloatSliderSetting(group, "ui.settings.mantleBlockHeight", 0f, 1f, 0.01f, delegate(float value)
				{
					Settings settings = app.Settings.Clone();
					settings.MantleBlockHeight = value;
					app.ApplyNewSettings(settings);
				});
			}
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x00078A8C File Offset: 0x00076C8C
		private void AddCameraSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.camera");
			this._flyCameraMode = this.AddCheckBoxSetting(group, "ui.settings.flyCameraMode", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.UseNewFlyCamera = value;
				app.ApplyNewSettings(settings);
			});
			this._sprintFovEffectSetting = this.AddCheckBoxSetting(group, "ui.settings.sprintFovEffect", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.SprintFovEffect = value;
				app.ApplyNewSettings(settings);
			});
			this._sprintFovIntensitySetting = this.AddFloatSliderSetting(group, "ui.settings.sprintFovIntensity", 1f, 1.5f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.SprintFovIntensity = value;
				app.ApplyNewSettings(settings);
			});
			this.AddSubSectionSpacer(group);
			this._viewBobbingEffectSetting = this.AddCheckBoxSetting(group, "ui.settings.viewBobbingEffect", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.ViewBobbingEffect = value;
				app.ApplyNewSettings(settings);
			});
			this._viewBobbingIntensitySetting = this.AddFloatSliderSetting(group, "ui.settings.viewBobbingIntensity", 0f, 1.5f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.ViewBobbingIntensity = value;
				app.ApplyNewSettings(settings);
			});
			this.AddSubSectionSpacer(group);
			this._cameraShakeEffectSetting = this.AddCheckBoxSetting(group, "ui.settings.cameraShakeEffect", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.CameraShakeEffect = value;
				app.ApplyNewSettings(settings);
			});
			this._firstPersonCameraShakeIntensitySetting = this.AddFloatSliderSetting(group, "ui.settings.firstPersonCameraShakeIntensity", 0f, 2f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.FirstPersonCameraShakeIntensity = value;
				app.ApplyNewSettings(settings);
			});
			this._thirdPersonCameraShakeIntensitySetting = this.AddFloatSliderSetting(group, "ui.settings.thirdPersonCameraShakeIntensity", 0f, 2f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.ThirdPersonCameraShakeIntensity = value;
				app.ApplyNewSettings(settings);
			});
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			foreach (string text in Enum.GetNames(typeof(Easing.EasingType)))
			{
				list.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
			}
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x00078C38 File Offset: 0x00076E38
		private void AddSprintForceSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.sprint");
			this._sprintForceSetting = this.AddCheckBoxSetting(group, "ui.settings.sprintForce", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.SprintForce = value;
				app.ApplyNewSettings(settings);
			});
			this._sprintAccelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.sprintAccelerationDuration", 0f, 2f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.SprintAccelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._sprintDecelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.sprintDecelerationDuration", 0f, 2f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.SprintDecelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._sprintAccelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.sprintAccelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.SprintAccelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
			this._sprintDecelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.sprintDecelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.SprintDecelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x00078D28 File Offset: 0x00076F28
		private void AddSlideForceSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.slide");
			this._slideDecelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.slideDecelerationDuration", 0f, 5f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.SlideDecelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._slideDecelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.slideDecelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.SlideDecelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00078DAC File Offset: 0x00076FAC
		private void AddStaminaSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.stamina");
			this._staminaLowAlertPercentSetting = this.AddSliderSetting(group, "ui.settings.staminaLowAlertPercent", 0, 100, 1, delegate(int value)
			{
				Settings settings = app.Settings.Clone();
				settings.StaminaLowAlertPercent = value;
				app.ApplyNewSettings(settings);
			});
			this._staminaDebugInfo = this.AddCheckBoxSetting(group, "ui.settings.staminaDebugInfo", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.StaminaDebugInfo = value;
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00078E1C File Offset: 0x0007701C
		private void AddWeaponPullbackSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.weaponPullback");
			this._weaponPullbackSetting = this.AddCheckBoxSetting(group, "ui.settings.weaponPullback", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.WeaponPullback = value;
				app.ApplyNewSettings(settings);
			});
			this._itemAnimationClippingSetting = this.AddCheckBoxSetting(group, "ui.settings.itemAnimationsClipGeometry", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.ItemAnimationsClipGeometry = value;
				app.ApplyNewSettings(settings);
			});
			this._itemClippingSetting = this.AddCheckBoxSetting(group, "ui.settings.itemsClipGeometry", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.ItemsClipGeometry = value;
				app.ApplyNewSettings(settings);
			});
			this._useOverrideFirstPersonAnimations = this.AddCheckBoxSetting(group, "ui.settings.overrideFirstPersonAnimations", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.UseOverrideFirstPersonAnimations = value;
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x00078EC4 File Offset: 0x000770C4
		private void AddEntityUISettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.entityui");
			this._entityUIMaxEntities = this.AddSliderSetting(group, "ui.settings.entityUIMaxEntities", 1, 100, 1, delegate(int value)
			{
				Settings settings = app.Settings.Clone();
				settings.EntityUIMaxEntities = value;
				app.ApplyNewSettings(settings);
			});
			this._entityUIMaxDistanceSetting = this.AddFloatSliderSetting(group, "ui.settings.entityUIMaxDistance", 1f, 64f, 1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.EntityUIMaxDistance = value;
				app.ApplyNewSettings(settings);
			});
			this._entityUIHideDelaySetting = this.AddFloatSliderSetting(group, "ui.settings.entityUIHideDelay", 0f, 6f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.EntityUIHideDelay = value;
				app.ApplyNewSettings(settings);
			});
			this._entityUIFadeInDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.entityUIFadeInDuration", 0f, 4f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.EntityUIFadeInDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._entityUIFadeOutDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.entityUIFadeOutDuration", 0f, 4f, 0.01f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.EntityUIFadeOutDuration = value;
				app.ApplyNewSettings(settings);
			});
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x00078FCC File Offset: 0x000771CC
		private void AddMountSettings(Group group, App app)
		{
			this.AddSectionSpacer(group);
			this.AddSectionHeader(group, "ui.settings.groups.mounts");
			this._mountMinTurnRate = this.AddFloatSliderSetting(group, "ui.settings.mountMinTurnRate", 0f, 360f, 1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountMinTurnRate = value;
				app.ApplyNewSettings(settings);
			});
			this._mountMinTurnRate.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountMinTurnRate.tooltip", null, true);
			this._mountMaxTurnRate = this.AddFloatSliderSetting(group, "ui.settings.mountMaxTurnRate", 0f, 360f, 1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountMaxTurnRate = value;
				app.ApplyNewSettings(settings);
			});
			this._mountMaxTurnRate.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountMaxTurnRate.tooltip", null, true);
			this._mountSpeedMinTurnRate = this.AddFloatSliderSetting(group, "ui.settings.mountSpeedMinTurnRate", 0f, 20f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountSpeedMinTurnRate = value;
				app.ApplyNewSettings(settings);
			});
			this._mountSpeedMinTurnRate.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountSpeedMinTurnRate.tooltip", null, true);
			this._mountSpeedMaxTurnRate = this.AddFloatSliderSetting(group, "ui.settings.mountSpeedMaxTurnRate", 0f, 20f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountSpeedMaxTurnRate = value;
				app.ApplyNewSettings(settings);
			});
			this._mountSpeedMaxTurnRate.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountSpeedMaxTurnRate.tooltip", null, true);
			this._mountForwardsAccelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.mountForwardsAccelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.MountForwardsAccelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
			this._mountForwardsAccelerationEasingTypeSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForwardsAccelerationEasingType.tooltip", null, true);
			this._mountForwardsDecelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.mountForwardsDecelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.MountForwardsDecelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
			this._mountForwardsDecelerationEasingTypeSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForwardsDecelerationEasingType.tooltip", null, true);
			this._mountBackwardsAccelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.mountBackwardsAccelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.MountBackwardsAccelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
			this._mountBackwardsAccelerationEasingTypeSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountBackwardsAccelerationEasingType.tooltip", null, true);
			this._mountBackwardsDecelerationEasingTypeSetting = this.AddDropdownSetting(group, "ui.settings.mountBackwardsDecelerationEasingType", this._easingTypes, delegate(string value)
			{
				Settings settings = app.Settings.Clone();
				Easing.EasingType easingType;
				settings.MountBackwardsDecelerationEasingType = (Enum.TryParse<Easing.EasingType>(value, out easingType) ? easingType : Easing.EasingType.Linear);
				app.ApplyNewSettings(settings);
			});
			this._mountBackwardsDecelerationEasingTypeSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountBackwardsDecelerationEasingType.tooltip", null, true);
			this._mountForwardsAccelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.mountForwardsAccelerationDuration", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountForwardsAccelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._mountForwardsAccelerationDurationSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForwardsAccelerationDuration.tooltip", null, true);
			this._mountForwardsDecelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.mountForwardsDecelerationDuration", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountForwardsDecelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._mountForwardsDecelerationDurationSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForwardsDecelerationDuration.tooltip", null, true);
			this._mountBackwardsAccelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.mountBackwardsAccelerationDuration", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountBackwardsAccelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._mountBackwardsAccelerationDurationSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountBackwardsAccelerationDuration.tooltip", null, true);
			this._mountBackwardsDecelerationDurationSetting = this.AddFloatSliderSetting(group, "ui.settings.mountBackwardsDecelerationDuration", 0f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountBackwardsDecelerationDuration = value;
				app.ApplyNewSettings(settings);
			});
			this._mountBackwardsDecelerationDurationSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountBackwardsDecelerationDuration.tooltip", null, true);
			this._mountForcedAccelerationMultiplierSetting = this.AddFloatSliderSetting(group, "ui.settings.mountForcedAccelerationMultiplierSetting", 1f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountForcedAccelerationMultiplier = value;
				app.ApplyNewSettings(settings);
			});
			this._mountForcedAccelerationMultiplierSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForcedAccelerationMultiplierSetting.tooltip", null, true);
			this._mountForcedDecelerationMultiplierSetting = this.AddFloatSliderSetting(group, "ui.settings.mountForcedDecelerationMultiplierSetting", 1f, 10f, 0.1f, delegate(float value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountForcedDecelerationMultiplier = value;
				app.ApplyNewSettings(settings);
			});
			this._mountForcedDecelerationMultiplierSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountForcedDecelerationMultiplierSetting.tooltip", null, true);
			this._mountRequireNewInputSetting = this.AddCheckBoxSetting(group, "ui.settings.mountRequireNewInput", delegate(bool value)
			{
				Settings settings = app.Settings.Clone();
				settings.MountRequireNewInput = value;
				app.ApplyNewSettings(settings);
			});
			this._mountRequireNewInputSetting.TooltipText = this.Desktop.Provider.GetText("ui.settings.mountRequireNewInput.tooltip", null, true);
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x00079479 File Offset: 0x00077679
		protected override void OnMounted()
		{
			this.GatherSettings();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0007949B File Offset: 0x0007769B
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x000794B8 File Offset: 0x000776B8
		private void Animate(float deltaTime)
		{
			bool flag = this._activeTab == SettingsComponent.SettingsTab.Audio;
			if (flag)
			{
				this._audioDeviceListRefreshCooldown += deltaTime;
				bool flag2 = this._audioDeviceListRefreshCooldown > 1f;
				if (flag2)
				{
					this._audioDeviceListRefreshCooldown = 0f;
					this.UpdateAudioOutputDeviceList();
				}
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x00079508 File Offset: 0x00077708
		public void StopEditingInputBinding()
		{
			this.Desktop.SetTransientLayer(null);
			this._editedInputBindingName = null;
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x00079520 File Offset: 0x00077720
		public void OnInputBindingKeyPress(SDL.SDL_Keycode keycode)
		{
			bool flag = this._editedInputBindingName == null;
			if (!flag)
			{
				this._inputBindingSettings[this._editedInputBindingName].SetValue(SDL.SDL_GetKeyName(keycode));
				this._inputBindingSettings[this._editedInputBindingName].Layout(null, true);
				Settings settings = this.Interface.App.Settings.Clone();
				InputBinding inputBinding = (InputBinding)typeof(InputBindings).GetField(this._editedInputBindingName).GetValue(settings.InputBindings);
				inputBinding.Keycode = new SDL.SDL_Keycode?(keycode);
				this.Interface.App.ApplyNewSettings(settings);
				this.Interface.TriggerEvent("settings.inputBindingsUpdated", null, null, null, null, null, null);
				this.StopEditingInputBinding();
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000795F8 File Offset: 0x000777F8
		public void OnInputBindingMousePress(Input.MouseButton button)
		{
			bool flag = this._editedInputBindingName == null;
			if (!flag)
			{
				this._inputBindingSettings[this._editedInputBindingName].SetValue(InputBinding.GetMouseBoundInputLabel(button));
				this._inputBindingSettings[this._editedInputBindingName].Layout(null, true);
				Settings settings = this.Interface.App.Settings.Clone();
				InputBinding inputBinding = (InputBinding)typeof(InputBindings).GetField(this._editedInputBindingName).GetValue(settings.InputBindings);
				inputBinding.MouseButton = new Input.MouseButton?(button);
				this.Interface.App.ApplyNewSettings(settings);
				this.Interface.TriggerEvent("settings.inputBindingsUpdated", null, null, null, null, null, null);
				this.StopEditingInputBinding();
			}
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000796D0 File Offset: 0x000778D0
		private void SelectTab(SettingsComponent.SettingsTab tab)
		{
			this._activeTab = tab;
			foreach (TextButton textButton in this._tabButtons.Values)
			{
				textButton.Style = this._buttonStyle;
			}
			this._tabButtons[this._activeTab].Style = this._buttonSelectedStyle;
			this.UpdateGroup();
			base.Layout(null, true);
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x00079770 File Offset: 0x00077970
		private void UpdateAudioOutputDeviceList()
		{
			SettingsComponent.<>c__DisplayClass135_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.audioDevices = new List<DropdownBox.DropdownEntryInfo>
			{
				new DropdownBox.DropdownEntryInfo(this.Desktop.Provider.GetText("ui.settings.defaultAudioDevice", null, true), "0", false)
			};
			AudioDevice audio = this.Interface.Engine.Audio;
			AudioDevice.OutputDevice[] outputDevices = audio.GetOutputDevices();
			for (int i = 0; i < audio.OutputDeviceCount; i++)
			{
				ref AudioDevice.OutputDevice ptr = ref outputDevices[i];
				CS$<>8__locals1.audioDevices.Add(new DropdownBox.DropdownEntryInfo(ptr.Name, ptr.Id.ToString(), false));
			}
			bool flag = false;
			CS$<>8__locals1.next = Enumerable.ToList<string>(Enumerable.Select<DropdownBox.DropdownEntryInfo, string>(CS$<>8__locals1.audioDevices, (DropdownBox.DropdownEntryInfo info) => info.Value));
			string value = this.Interface.App.Settings.AudioSettings.OutputDeviceId.ToString();
			bool flag2 = !CS$<>8__locals1.next.Contains(this._outputDeviceSetting.Dropdown.Value);
			if (flag2)
			{
				value = 0U.ToString();
				flag = true;
			}
			bool flag3 = !flag && !this.<UpdateAudioOutputDeviceList>g__HasListChanged|135_1(ref CS$<>8__locals1);
			if (!flag3)
			{
				this._outputDeviceSetting.SetEntries(CS$<>8__locals1.audioDevices);
				this._outputDeviceSetting.SetValue(value);
				this._outputDeviceSetting.Layout(null, true);
			}
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x00079900 File Offset: 0x00077B00
		private void UpdateGroup()
		{
			foreach (Group group in this._tabs.Values)
			{
				group.Visible = false;
			}
			Group group2;
			bool flag = this._tabs.TryGetValue(this._activeTab, out group2);
			if (flag)
			{
				group2.Visible = true;
			}
			bool flag2 = this._activeTab == SettingsComponent.SettingsTab.Controls || this._activeTab == SettingsComponent.SettingsTab.Prototype;
			if (flag2)
			{
				this._tabContainer.LayoutMode = LayoutMode.TopScrolling;
			}
			else
			{
				this._tabContainer.LayoutMode = LayoutMode.Top;
			}
			this._tabContainer.SetScroll(new int?(0), new int?(0));
			bool isMounted = this._tabContainer.IsMounted;
			if (isMounted)
			{
				this._tabContainer.Layout(null, true);
			}
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000799FC File Offset: 0x00077BFC
		private void AddSectionSpacer(Group container)
		{
			new Element(this.Desktop, container).Anchor = new Anchor
			{
				Height = new int?(50)
			};
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x00079A34 File Offset: 0x00077C34
		private void AddSubSectionSpacer(Group container)
		{
			new Element(this.Desktop, container).Anchor = new Anchor
			{
				Height = new int?(15)
			};
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x00079A6C File Offset: 0x00077C6C
		private void AddSectionHeader(Group container, string name)
		{
			Document document;
			this.Interface.TryGetDocument("Common/Settings/SectionHeader.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, container);
			uifragment.Get<Label>("Name").Text = this.Desktop.Provider.GetText(name, null, true);
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00079AC0 File Offset: 0x00077CC0
		private InputBindingSettingComponent AddInputBinding(Group container, string name)
		{
			return new InputBindingSettingComponent(this.Desktop, container, name, this)
			{
				OnChange = delegate(string _)
				{
					this._editedInputBindingName = name;
					this._inputBindingPopup.Setup(name);
					this.Desktop.SetTransientLayer(this._inputBindingPopup);
				}
			};
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x00079B0C File Offset: 0x00077D0C
		private LabeledCheckBoxSettingComponent AddCheckBoxSetting(Group container, string name, Action<bool> onChange)
		{
			return new LabeledCheckBoxSettingComponent(this.Desktop, container, name, this)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x00079B34 File Offset: 0x00077D34
		private SliderSettingComponent AddSliderSetting(Group container, string name, int min, int max, int step, Action<int> onChange)
		{
			return new SliderSettingComponent(this.Desktop, container, name, this, min, max, step)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x00079B64 File Offset: 0x00077D64
		private FloatSliderSettingComponent AddFloatSliderSetting(Group container, string name, float min, float max, float step, Action<float> onChange)
		{
			return new FloatSliderSettingComponent(this.Desktop, container, name, this, min, max, step)
			{
				OnChange = onChange
			};
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x00079B94 File Offset: 0x00077D94
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

		// Token: 0x06003949 RID: 14665 RVA: 0x00079C00 File Offset: 0x00077E00
		private DropdownSettingComponent AddDropdownSetting(Group container, string name, List<KeyValuePair<string, string>> values, Action<string> onChange)
		{
			return new DropdownSettingComponent(this.Desktop, container, name, this, Enumerable.ToList<DropdownBox.DropdownEntryInfo>(Enumerable.Select<KeyValuePair<string, string>, DropdownBox.DropdownEntryInfo>(values, (KeyValuePair<string, string> e) => new DropdownBox.DropdownEntryInfo(e.Key, e.Value.ToString(), false))))
			{
				OnChange = onChange
			};
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x00079C54 File Offset: 0x00077E54
		private DropdownSettingComponent AddDropdownSetting(Group container, string name, List<DropdownBox.DropdownEntryInfo> values, Action<string> onChange)
		{
			return new DropdownSettingComponent(this.Desktop, container, name, this, values)
			{
				OnChange = onChange
			};
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x00079C80 File Offset: 0x00077E80
		public void SetHoveredSetting<T>(string setting, SettingComponent<T> component)
		{
			bool flag = setting == null;
			if (flag)
			{
				this._settingInfo.Visible = false;
			}
			else
			{
				this._settingInfoLabel.Text = setting;
				this._settingInfo.Anchor.Left = new int?(this.Desktop.UnscaleRound((float)component.Children[0].ContainerRectangle.Right));
				this._settingInfo.Anchor.Top = new int?(this.Desktop.UnscaleRound((float)component.Children[0].ContainerRectangle.Top));
				this._settingInfo.Visible = true;
				this._settingInfo.Layout(new Rectangle?(this.Desktop.RootLayoutRectangle), true);
			}
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x00079D54 File Offset: 0x00077F54
		public bool TryGetDocument(string path, out Document document)
		{
			return this.Desktop.Provider.TryGetDocument("Common/Settings/" + path, out document);
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x00079D84 File Offset: 0x00077F84
		public void GatherSettings()
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				Settings settings = this.Interface.App.Settings;
				this._fullscreenSetting.SetValue(settings.Fullscreen ? (settings.UseBorderlessForFullscreen ? SettingsComponent.WindowMode.WindowedFullscreen : SettingsComponent.WindowMode.Fullscreen).ToString() : SettingsComponent.WindowMode.Window.ToString());
				this._maxChatMessagesSetting.SetValue(settings.MaxChatMessages);
				this._diagnosticsModeSetting.SetValue(settings.DiagnosticMode);
				this._vsyncSetting.SetValue(settings.VSync);
				this._unlimitedFpsSetting.SetValue(settings.UnlimitedFps);
				this._maxFpsSetting.Visible = !settings.UnlimitedFps;
				this._maxFpsSetting.SetValue(settings.FpsLimit);
				this._viewDistanceSetting.SetValue(settings.ViewDistance);
				this._fieldOfViewSetting.SetValue(settings.FieldOfView);
				this._automaticRenderScaleSetting.SetValue(settings.AutomaticRenderScale);
				this._renderScaleSetting.Visible = !settings.AutomaticRenderScale;
				this._renderScaleSetting.SetValue(settings.RenderScale);
				this._placementPreviewSetting.SetValue(settings.PlacementPreviewMode.ToString());
				this._resolutionSetting.SetValue(settings.ScreenResolution.ToString());
				this._outputDeviceSetting.SetValue(settings.AudioSettings.OutputDeviceId.ToString());
				this._masterVolumeSetting.SetValue((int)settings.AudioSettings.MasterVolume);
				this._musicVolumeSetting.SetValue((int)settings.AudioSettings.CategoryVolumes["MusicVolume"]);
				this._ambientVolumeSetting.SetValue((int)settings.AudioSettings.CategoryVolumes["AmbienceVolume"]);
				this._uiVolumeSetting.SetValue((int)settings.AudioSettings.CategoryVolumes["UIVolume"]);
				this._sfxVolumeSetting.SetValue((int)settings.AudioSettings.CategoryVolumes["SFXVolume"]);
				this._mouseRawInputModeSetting.SetValue(settings.MouseSettings.MouseRawInputMode);
				this._mouseInvertedSetting.SetValue(settings.MouseSettings.MouseInverted);
				this._mouseXSpeedSetting.SetValue(settings.MouseSettings.MouseXSpeed);
				this._mouseYSpeedSetting.SetValue(settings.MouseSettings.MouseYSpeed);
				foreach (FieldInfo fieldInfo in typeof(InputBindings).GetFields(BindingFlags.Instance | BindingFlags.Public))
				{
					bool flag = fieldInfo.FieldType == typeof(InputBinding);
					if (flag)
					{
						SettingComponent<string> settingComponent = this._inputBindingSettings[fieldInfo.Name];
						InputBinding inputBinding = (InputBinding)fieldInfo.GetValue(settings.InputBindings);
						settingComponent.SetValue((inputBinding != null) ? inputBinding.BoundInputLabel : null);
					}
				}
				this._builderToolsUseToolReachDistanceSetting.SetValue(settings.BuilderToolsSettings.useToolReachDistance);
				this._builderToolsToolReachDistanceSetting.SetValue(settings.BuilderToolsSettings.ToolReachDistance);
				this._builderToolsToolReachDistanceSetting.Visible = settings.BuilderToolsSettings.useToolReachDistance;
				this._builderToolsToolReachLockSetting.SetValue(settings.BuilderToolsSettings.ToolReachLock);
				this._builderToolsToolDelayMinSetting.SetValue(settings.BuilderToolsSettings.ToolDelayMin);
				this._builderToolsEnableBrushShapeRenderingSetting.SetValue(settings.BuilderToolsSettings.EnableBrushShapeRendering);
				this._builderToolsBrushOpacitySetting.SetValue(settings.BuilderToolsSettings.BrushOpacity);
				this._builderToolsSelectionOpacitySetting.SetValue(settings.BuilderToolsSettings.SelectionOpacity);
				this._builderToolsDisplayLegendSetting.SetValue(settings.BuilderToolsSettings.DisplayLegend);
				this._showBuilderToolsNotificationsSetting.SetValue(settings.BuilderToolsSettings.ShowBuilderToolsNotifications);
				this._showDebugMarkersSetting.SetValue(settings.DebugSettings.ShowDebugMarkers);
				this._languageSetting.SetValue(settings.Language ?? "");
				this._inputBindingSettings["OpenDevTools"].Visible = settings.DiagnosticMode;
				this._autoJumpObstacleSetting.SetValue(settings.AutoJumpObstacle);
				this._autoJumpGap.SetValue(settings.AutoJumpGap);
				this._builderModeSetting.SetValue(settings.BuilderMode);
				this._blockHealthSetting.SetValue(settings.BlockHealth);
				this._creativeInteractionDistance.SetValue(settings.creativeInteractionDistance);
				this._adventureInteractionDistance.SetValue(settings.adventureInteractionDistance);
				this._mantlingSetting.SetValue(settings.Mantling);
				this._minVelocityMantlingSetting.SetValue(settings.MinVelocityMantling);
				this._maxVelocityMantlingSetting.SetValue(settings.MaxVelocityMantling);
				this._jumpForceSpeedMultiplierStep.SetValue(settings.JumpForceSpeedMultiplierStep);
				this._maxJumpForceSpeedMultiplier.SetValue(settings.MaxJumpForceSpeedMultiplier);
				this._mantlingCameraOffsetY.SetValue(settings.MantlingCameraOffsetY);
				this._mantleBlockHeight.SetValue(settings.MantleBlockHeight);
				this._sprintFovEffectSetting.SetValue(settings.SprintFovEffect);
				this._sprintFovIntensitySetting.SetValue(settings.SprintFovIntensity);
				this._viewBobbingEffectSetting.SetValue(settings.ViewBobbingEffect);
				this._viewBobbingIntensitySetting.SetValue(settings.ViewBobbingIntensity);
				this._cameraShakeEffectSetting.SetValue(settings.CameraShakeEffect);
				this._firstPersonCameraShakeIntensitySetting.SetValue(settings.FirstPersonCameraShakeIntensity);
				this._thirdPersonCameraShakeIntensitySetting.SetValue(settings.ThirdPersonCameraShakeIntensity);
				this._sprintForceSetting.SetValue(settings.SprintForce);
				this._sprintAccelerationEasingTypeSetting.SetValue(settings.SprintAccelerationEasingType.ToString());
				this._sprintAccelerationDurationSetting.SetValue(settings.SprintAccelerationDuration);
				this._sprintDecelerationEasingTypeSetting.SetValue(settings.SprintDecelerationEasingType.ToString());
				this._sprintDecelerationDurationSetting.SetValue(settings.SprintDecelerationDuration);
				this._flyCameraMode.SetValue(settings.UseNewFlyCamera);
				this._slideDecelerationDurationSetting.SetValue(settings.SlideDecelerationDuration);
				this._slideDecelerationEasingTypeSetting.SetValue(settings.SlideDecelerationEasingType.ToString());
				this._staminaLowAlertPercentSetting.SetValue(settings.StaminaLowAlertPercent);
				this._staminaDebugInfo.SetValue(settings.StaminaDebugInfo);
				this._weaponPullbackSetting.SetValue(settings.WeaponPullback);
				this._itemAnimationClippingSetting.SetValue(settings.ItemAnimationsClipGeometry);
				this._itemClippingSetting.SetValue(settings.ItemsClipGeometry);
				this._useOverrideFirstPersonAnimations.SetValue(settings.UseOverrideFirstPersonAnimations);
				this._entityUIMaxEntities.SetValue(settings.EntityUIMaxEntities);
				this._entityUIMaxDistanceSetting.SetValue(settings.EntityUIMaxDistance);
				this._entityUIHideDelaySetting.SetValue(settings.EntityUIHideDelay);
				this._entityUIFadeInDurationSetting.SetValue(settings.EntityUIFadeInDuration);
				this._entityUIFadeOutDurationSetting.SetValue(settings.EntityUIFadeOutDuration);
				this._useBlockSubfaces.SetValue(settings.UseBlockSubfaces);
				this._displayBlockSubfaces.SetValue(settings.DisplayBlockSubfaces);
				this._displayBlockSubfaces.Visible = settings.UseBlockSubfaces;
				this._displayBlockBoundaries.SetValue(settings.DisplayBlockBoundaries);
				this._placeBlocksAtRange.SetValue(settings._placeBlocksAtRange);
				this._placeBlocksAtRangeInAdventureMode.SetValue(settings.PlaceBlocksAtRangeInAdventureMode);
				this._placeBlocksAtRangeInAdventureMode.Visible = settings._placeBlocksAtRange;
				this._blockPlacementSupportValidation.SetValue(settings.BlockPlacementSupportValidation);
				this._resetMouseSensitivityDuration.SetValue(settings.ResetMouseSensitivityDuration);
				this._percentageOfPlaySelectionLengthGizmoShouldRenderSetting.SetValue((int)(settings.PercentageOfPlaySelectionLengthGizmoShouldRender * 100f));
				this._minPlaySelectGizmoSizeSetting.SetValue(settings.MinPlaySelectGizmoSize);
				this._maxPlaySelectGizmoSizeSetting.SetValue(settings.MaxPlaySelectGizmoSize);
				this._builderToolsPaintOperationsIgnoreHistoryLengthSetting.SetValue(settings.PaintOperationsIgnoreHistoryLength);
				this._builderToolsSpacingBlocksOffset.SetValue(settings.BrushSpacingBlocks);
				this._builderToolEnableBrushSpacing.SetValue(settings.EnableBrushSpacing);
				this._mountMinTurnRate.SetValue(settings.MountMinTurnRate);
				this._mountMaxTurnRate.SetValue(settings.MountMaxTurnRate);
				this._mountSpeedMinTurnRate.SetValue(settings.MountSpeedMinTurnRate);
				this._mountSpeedMaxTurnRate.SetValue(settings.MountSpeedMaxTurnRate);
				this._mountForwardsAccelerationEasingTypeSetting.SetValue(settings.MountForwardsAccelerationEasingType.ToString());
				this._mountForwardsDecelerationEasingTypeSetting.SetValue(settings.MountForwardsDecelerationEasingType.ToString());
				this._mountBackwardsAccelerationEasingTypeSetting.SetValue(settings.MountBackwardsAccelerationEasingType.ToString());
				this._mountBackwardsDecelerationEasingTypeSetting.SetValue(settings.MountBackwardsDecelerationEasingType.ToString());
				this._mountForwardsAccelerationDurationSetting.SetValue(settings.MountForwardsAccelerationDuration);
				this._mountForwardsDecelerationDurationSetting.SetValue(settings.MountForwardsDecelerationDuration);
				this._mountBackwardsAccelerationDurationSetting.SetValue(settings.MountBackwardsAccelerationDuration);
				this._mountBackwardsDecelerationDurationSetting.SetValue(settings.MountBackwardsDecelerationDuration);
				this._mountForcedAccelerationMultiplierSetting.SetValue(settings.MountForcedAccelerationMultiplier);
				this._mountForcedDecelerationMultiplierSetting.SetValue(settings.MountForcedDecelerationMultiplier);
				this._mountRequireNewInputSetting.SetValue(settings.MountRequireNewInput);
				base.Layout(null, true);
			}
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x0007A70C File Offset: 0x0007890C
		public void OnWindowSizeChanged()
		{
			bool flag = this._resolutionSetting == null;
			if (!flag)
			{
				List<ScreenResolution> availableResolutions = ScreenResolutions.GetAvailableResolutions(this.Interface.App);
				Point size = this.Interface.Engine.Window.GetSize();
				bool flag2 = !availableResolutions.Contains(new ScreenResolution(size.X, size.Y, this.Interface.Engine.Window.GetState() == Window.WindowState.Fullscreen));
				if (flag2)
				{
					this._resolutionSetting.SetValue(ScreenResolutions.CustomScreenResolution.ToString());
				}
			}
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x0007A7A4 File Offset: 0x000789A4
		[CompilerGenerated]
		private bool <UpdateAudioOutputDeviceList>g__HasListChanged|135_1(ref SettingsComponent.<>c__DisplayClass135_0 A_1)
		{
			IReadOnlyList<DropdownBox.DropdownEntryInfo> entries = this._outputDeviceSetting.Dropdown.Entries;
			bool flag = A_1.audioDevices.Count != entries.Count;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				foreach (DropdownBox.DropdownEntryInfo dropdownEntryInfo in entries)
				{
					bool flag2 = !A_1.next.Contains(dropdownEntryInfo.Value);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x040018CA RID: 6346
		private SettingsComponent.SettingsTab _activeTab = SettingsComponent.SettingsTab.General;

		// Token: 0x040018CB RID: 6347
		private Group _settingInfo;

		// Token: 0x040018CC RID: 6348
		private Label _settingInfoLabel;

		// Token: 0x040018CD RID: 6349
		private Group _tabContainer;

		// Token: 0x040018CE RID: 6350
		private TextButton.TextButtonStyle _buttonStyle;

		// Token: 0x040018CF RID: 6351
		private TextButton.TextButtonStyle _buttonSelectedStyle;

		// Token: 0x040018D0 RID: 6352
		private Dictionary<SettingsComponent.SettingsTab, TextButton> _tabButtons = new Dictionary<SettingsComponent.SettingsTab, TextButton>();

		// Token: 0x040018D1 RID: 6353
		private Dictionary<SettingsComponent.SettingsTab, Group> _tabs = new Dictionary<SettingsComponent.SettingsTab, Group>();

		// Token: 0x040018D2 RID: 6354
		private DropdownSettingComponent _languageSetting;

		// Token: 0x040018D3 RID: 6355
		private DropdownSettingComponent _fullscreenSetting;

		// Token: 0x040018D4 RID: 6356
		private LabeledCheckBoxSettingComponent _diagnosticsModeSetting;

		// Token: 0x040018D5 RID: 6357
		private SliderSettingComponent _maxChatMessagesSetting;

		// Token: 0x040018D6 RID: 6358
		private LabeledCheckBoxSettingComponent _autoJumpObstacleSetting;

		// Token: 0x040018D7 RID: 6359
		private LabeledCheckBoxSettingComponent _autoJumpGap;

		// Token: 0x040018D8 RID: 6360
		private FloatSliderSettingComponent _jumpForceSpeedMultiplierStep;

		// Token: 0x040018D9 RID: 6361
		private FloatSliderSettingComponent _maxJumpForceSpeedMultiplier;

		// Token: 0x040018DA RID: 6362
		private LabeledCheckBoxSettingComponent _builderModeSetting;

		// Token: 0x040018DB RID: 6363
		private LabeledCheckBoxSettingComponent _blockHealthSetting;

		// Token: 0x040018DC RID: 6364
		private SliderSettingComponent _creativeInteractionDistance;

		// Token: 0x040018DD RID: 6365
		private SliderSettingComponent _adventureInteractionDistance;

		// Token: 0x040018DE RID: 6366
		private LabeledCheckBoxSettingComponent _mantlingSetting;

		// Token: 0x040018DF RID: 6367
		private FloatSliderSettingComponent _minVelocityMantlingSetting;

		// Token: 0x040018E0 RID: 6368
		private FloatSliderSettingComponent _maxVelocityMantlingSetting;

		// Token: 0x040018E1 RID: 6369
		private FloatSliderSettingComponent _mantlingCameraOffsetY;

		// Token: 0x040018E2 RID: 6370
		private FloatSliderSettingComponent _mantleBlockHeight;

		// Token: 0x040018E3 RID: 6371
		private LabeledCheckBoxSettingComponent _useBlockSubfaces;

		// Token: 0x040018E4 RID: 6372
		private LabeledCheckBoxSettingComponent _displayBlockSubfaces;

		// Token: 0x040018E5 RID: 6373
		private LabeledCheckBoxSettingComponent _displayBlockBoundaries;

		// Token: 0x040018E6 RID: 6374
		private LabeledCheckBoxSettingComponent _placeBlocksAtRange;

		// Token: 0x040018E7 RID: 6375
		private LabeledCheckBoxSettingComponent _placeBlocksAtRangeInAdventureMode;

		// Token: 0x040018E8 RID: 6376
		private LabeledCheckBoxSettingComponent _blockPlacementSupportValidation;

		// Token: 0x040018E9 RID: 6377
		private FloatSliderSettingComponent _resetMouseSensitivityDuration;

		// Token: 0x040018EA RID: 6378
		private DropdownSettingComponent _resolutionSetting;

		// Token: 0x040018EB RID: 6379
		private LabeledCheckBoxSettingComponent _vsyncSetting;

		// Token: 0x040018EC RID: 6380
		private LabeledCheckBoxSettingComponent _unlimitedFpsSetting;

		// Token: 0x040018ED RID: 6381
		private SliderSettingComponent _maxFpsSetting;

		// Token: 0x040018EE RID: 6382
		private SliderSettingComponent _viewDistanceSetting;

		// Token: 0x040018EF RID: 6383
		private SliderSettingComponent _fieldOfViewSetting;

		// Token: 0x040018F0 RID: 6384
		private LabeledCheckBoxSettingComponent _automaticRenderScaleSetting;

		// Token: 0x040018F1 RID: 6385
		private SliderSettingComponent _renderScaleSetting;

		// Token: 0x040018F2 RID: 6386
		private DropdownSettingComponent _placementPreviewSetting;

		// Token: 0x040018F3 RID: 6387
		private DropdownSettingComponent _outputDeviceSetting;

		// Token: 0x040018F4 RID: 6388
		private SliderSettingComponent _masterVolumeSetting;

		// Token: 0x040018F5 RID: 6389
		private SliderSettingComponent _musicVolumeSetting;

		// Token: 0x040018F6 RID: 6390
		private SliderSettingComponent _ambientVolumeSetting;

		// Token: 0x040018F7 RID: 6391
		private SliderSettingComponent _uiVolumeSetting;

		// Token: 0x040018F8 RID: 6392
		private SliderSettingComponent _sfxVolumeSetting;

		// Token: 0x040018F9 RID: 6393
		private LabeledCheckBoxSettingComponent _mouseRawInputModeSetting;

		// Token: 0x040018FA RID: 6394
		private LabeledCheckBoxSettingComponent _mouseInvertedSetting;

		// Token: 0x040018FB RID: 6395
		private FloatSliderSettingComponent _mouseXSpeedSetting;

		// Token: 0x040018FC RID: 6396
		private FloatSliderSettingComponent _mouseYSpeedSetting;

		// Token: 0x040018FD RID: 6397
		private LabeledCheckBoxSettingComponent _builderToolsUseToolReachDistanceSetting;

		// Token: 0x040018FE RID: 6398
		private SliderSettingComponent _builderToolsToolReachDistanceSetting;

		// Token: 0x040018FF RID: 6399
		private LabeledCheckBoxSettingComponent _builderToolsToolReachLockSetting;

		// Token: 0x04001900 RID: 6400
		private SliderSettingComponent _builderToolsToolDelayMinSetting;

		// Token: 0x04001901 RID: 6401
		private LabeledCheckBoxSettingComponent _builderToolsEnableBrushShapeRenderingSetting;

		// Token: 0x04001902 RID: 6402
		private SliderSettingComponent _builderToolsBrushOpacitySetting;

		// Token: 0x04001903 RID: 6403
		private SliderSettingComponent _builderToolsSelectionOpacitySetting;

		// Token: 0x04001904 RID: 6404
		private LabeledCheckBoxSettingComponent _builderToolsDisplayLegendSetting;

		// Token: 0x04001905 RID: 6405
		private SliderSettingComponent _percentageOfPlaySelectionLengthGizmoShouldRenderSetting;

		// Token: 0x04001906 RID: 6406
		private SliderSettingComponent _minPlaySelectGizmoSizeSetting;

		// Token: 0x04001907 RID: 6407
		private SliderSettingComponent _maxPlaySelectGizmoSizeSetting;

		// Token: 0x04001908 RID: 6408
		private LabeledCheckBoxSettingComponent _sprintFovEffectSetting;

		// Token: 0x04001909 RID: 6409
		private FloatSliderSettingComponent _sprintFovIntensitySetting;

		// Token: 0x0400190A RID: 6410
		private LabeledCheckBoxSettingComponent _viewBobbingEffectSetting;

		// Token: 0x0400190B RID: 6411
		private FloatSliderSettingComponent _viewBobbingIntensitySetting;

		// Token: 0x0400190C RID: 6412
		private LabeledCheckBoxSettingComponent _flyCameraMode;

		// Token: 0x0400190D RID: 6413
		private LabeledCheckBoxSettingComponent _cameraShakeEffectSetting;

		// Token: 0x0400190E RID: 6414
		private FloatSliderSettingComponent _firstPersonCameraShakeIntensitySetting;

		// Token: 0x0400190F RID: 6415
		private FloatSliderSettingComponent _thirdPersonCameraShakeIntensitySetting;

		// Token: 0x04001910 RID: 6416
		private SliderSettingComponent _builderToolsPaintOperationsIgnoreHistoryLengthSetting;

		// Token: 0x04001911 RID: 6417
		private SliderSettingComponent _builderToolsSpacingBlocksOffset;

		// Token: 0x04001912 RID: 6418
		private LabeledCheckBoxSettingComponent _builderToolEnableBrushSpacing;

		// Token: 0x04001913 RID: 6419
		private LabeledCheckBoxSettingComponent _sprintForceSetting;

		// Token: 0x04001914 RID: 6420
		private DropdownSettingComponent _sprintAccelerationEasingTypeSetting;

		// Token: 0x04001915 RID: 6421
		private FloatSliderSettingComponent _sprintAccelerationDurationSetting;

		// Token: 0x04001916 RID: 6422
		private DropdownSettingComponent _sprintDecelerationEasingTypeSetting;

		// Token: 0x04001917 RID: 6423
		private FloatSliderSettingComponent _sprintDecelerationDurationSetting;

		// Token: 0x04001918 RID: 6424
		private DropdownSettingComponent _slideDecelerationEasingTypeSetting;

		// Token: 0x04001919 RID: 6425
		private FloatSliderSettingComponent _slideDecelerationDurationSetting;

		// Token: 0x0400191A RID: 6426
		private SliderSettingComponent _staminaLowAlertPercentSetting;

		// Token: 0x0400191B RID: 6427
		private LabeledCheckBoxSettingComponent _staminaDebugInfo;

		// Token: 0x0400191C RID: 6428
		private LabeledCheckBoxSettingComponent _weaponPullbackSetting;

		// Token: 0x0400191D RID: 6429
		private LabeledCheckBoxSettingComponent _itemAnimationClippingSetting;

		// Token: 0x0400191E RID: 6430
		private LabeledCheckBoxSettingComponent _itemClippingSetting;

		// Token: 0x0400191F RID: 6431
		private LabeledCheckBoxSettingComponent _useOverrideFirstPersonAnimations;

		// Token: 0x04001920 RID: 6432
		private SliderSettingComponent _entityUIMaxEntities;

		// Token: 0x04001921 RID: 6433
		private FloatSliderSettingComponent _entityUIMaxDistanceSetting;

		// Token: 0x04001922 RID: 6434
		private FloatSliderSettingComponent _entityUIHideDelaySetting;

		// Token: 0x04001923 RID: 6435
		private FloatSliderSettingComponent _entityUIFadeInDurationSetting;

		// Token: 0x04001924 RID: 6436
		private FloatSliderSettingComponent _entityUIFadeOutDurationSetting;

		// Token: 0x04001925 RID: 6437
		private LabeledCheckBoxSettingComponent _showBuilderToolsNotificationsSetting;

		// Token: 0x04001926 RID: 6438
		private LabeledCheckBoxSettingComponent _showDebugMarkersSetting;

		// Token: 0x04001927 RID: 6439
		private FloatSliderSettingComponent _mountMinTurnRate;

		// Token: 0x04001928 RID: 6440
		private FloatSliderSettingComponent _mountMaxTurnRate;

		// Token: 0x04001929 RID: 6441
		private FloatSliderSettingComponent _mountSpeedMinTurnRate;

		// Token: 0x0400192A RID: 6442
		private FloatSliderSettingComponent _mountSpeedMaxTurnRate;

		// Token: 0x0400192B RID: 6443
		private DropdownSettingComponent _mountForwardsAccelerationEasingTypeSetting;

		// Token: 0x0400192C RID: 6444
		private DropdownSettingComponent _mountForwardsDecelerationEasingTypeSetting;

		// Token: 0x0400192D RID: 6445
		private DropdownSettingComponent _mountBackwardsAccelerationEasingTypeSetting;

		// Token: 0x0400192E RID: 6446
		private DropdownSettingComponent _mountBackwardsDecelerationEasingTypeSetting;

		// Token: 0x0400192F RID: 6447
		private FloatSliderSettingComponent _mountForwardsAccelerationDurationSetting;

		// Token: 0x04001930 RID: 6448
		private FloatSliderSettingComponent _mountForwardsDecelerationDurationSetting;

		// Token: 0x04001931 RID: 6449
		private FloatSliderSettingComponent _mountBackwardsAccelerationDurationSetting;

		// Token: 0x04001932 RID: 6450
		private FloatSliderSettingComponent _mountBackwardsDecelerationDurationSetting;

		// Token: 0x04001933 RID: 6451
		private FloatSliderSettingComponent _mountForcedAccelerationMultiplierSetting;

		// Token: 0x04001934 RID: 6452
		private FloatSliderSettingComponent _mountForcedDecelerationMultiplierSetting;

		// Token: 0x04001935 RID: 6453
		private LabeledCheckBoxSettingComponent _mountRequireNewInputSetting;

		// Token: 0x04001936 RID: 6454
		private Dictionary<string, InputBindingSettingComponent> _inputBindingSettings = new Dictionary<string, InputBindingSettingComponent>();

		// Token: 0x04001937 RID: 6455
		private InputBindingPopup _inputBindingPopup;

		// Token: 0x04001938 RID: 6456
		private string _editedInputBindingName;

		// Token: 0x04001939 RID: 6457
		private float _audioDeviceListRefreshCooldown;

		// Token: 0x0400193A RID: 6458
		private List<DropdownBox.DropdownEntryInfo> _easingTypes;

		// Token: 0x02000CDC RID: 3292
		private enum SettingsTab
		{
			// Token: 0x04004016 RID: 16406
			General,
			// Token: 0x04004017 RID: 16407
			Visual,
			// Token: 0x04004018 RID: 16408
			Audio,
			// Token: 0x04004019 RID: 16409
			Mouse,
			// Token: 0x0400401A RID: 16410
			Controls,
			// Token: 0x0400401B RID: 16411
			Creative,
			// Token: 0x0400401C RID: 16412
			Prototype
		}

		// Token: 0x02000CDD RID: 3293
		private enum WindowMode
		{
			// Token: 0x0400401E RID: 16414
			Window,
			// Token: 0x0400401F RID: 16415
			Fullscreen,
			// Token: 0x04004020 RID: 16416
			WindowedFullscreen
		}
	}
}
