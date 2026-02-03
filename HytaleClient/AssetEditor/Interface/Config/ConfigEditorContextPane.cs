using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Interface.Previews;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBD RID: 3005
	internal class ConfigEditorContextPane : Element
	{
		// Token: 0x06005E40 RID: 24128 RVA: 0x001E1C6E File Offset: 0x001DFE6E
		public ConfigEditorContextPane(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this.DayTimeControls = new DayTimeControls(assetEditorOverlay);
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x001E1CA0 File Offset: 0x001DFEA0
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ConfigEditorContextPane.ui", out document);
			Document document2;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ConfigEditorContextPaneSectionButton.ui", out document2);
			this._style = document2.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "Style");
			this._activeStyle = document2.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "ActiveStyle");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._buttonsGroup = uifragment.Get<Group>("Buttons");
			this._sectionsGroup = uifragment.Get<Group>("Sections");
			this._previewGroup = uifragment.Get<DynamicPane>("PreviewGroup");
			this._previewGroup.MouseButtonReleased = delegate()
			{
				AssetTypeConfig assetTypeConfig;
				bool flag = this._assetEditorOverlay.CurrentAsset.Type != null && this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(this._assetEditorOverlay.CurrentAsset.Type, out assetTypeConfig);
				if (flag)
				{
					AssetTypeConfig.PreviewType preview = assetTypeConfig.Preview;
					AssetTypeConfig.PreviewType previewType = preview;
					if (previewType != AssetTypeConfig.PreviewType.Item)
					{
						if (previewType == AssetTypeConfig.PreviewType.Model)
						{
							this._assetEditorOverlay.UpdatePaneSize(AssetEditorSettings.Panes.ConfigEditorSidebarPreviewModel, this._previewGroup.Anchor.Height.Value);
						}
					}
					else
					{
						this._assetEditorOverlay.UpdatePaneSize(AssetEditorSettings.Panes.ConfigEditorSidebarPreviewItem, this._previewGroup.Anchor.Height.Value);
					}
				}
			};
			this._modelPreview = new ModelPreview(this._assetEditorOverlay, this._previewGroup);
			this._modelPreview.Visible = false;
			this._blockPreview = new BlockPreview(this._assetEditorOverlay, this._previewGroup);
			this._blockPreview.Visible = false;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Update();
			}
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x001E1DD0 File Offset: 0x001DFFD0
		public void SetActiveCategory(string key, bool doLayout = true)
		{
			TextButton textButton;
			bool flag = this._activeCategory != null && this._sectionButtons.TryGetValue(this._activeCategory, out textButton);
			if (flag)
			{
				textButton.Style = this._style;
				if (doLayout)
				{
					textButton.Layout(null, true);
				}
			}
			this._activeCategory = key;
			TextButton textButton2;
			bool flag2 = this._activeCategory != null && this._sectionButtons.TryGetValue(this._activeCategory, out textButton2);
			if (flag2)
			{
				textButton2.Style = this._activeStyle;
				if (doLayout)
				{
					textButton2.Layout(null, true);
				}
			}
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x001E1E77 File Offset: 0x001E0077
		private void SendAction(string action)
		{
			this._assetEditorOverlay.Backend.OnSidebarButtonActivated(action);
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x001E1E8C File Offset: 0x001E008C
		public void ResetState()
		{
			this.DayTimeControls.ResetState();
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x001E1E9C File Offset: 0x001E009C
		public void Update()
		{
			this._sectionButtons.Clear();
			this._sectionsGroup.Clear();
			this._buttonsGroup.Clear();
			AssetTypeConfig assetTypeConfig;
			bool flag = this._assetEditorOverlay.CurrentAsset.Type != null && this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(this._assetEditorOverlay.CurrentAsset.Type, out assetTypeConfig);
			if (flag)
			{
				bool flag2 = this._assetEditorOverlay.Mode == AssetEditorOverlay.EditorMode.Editor && assetTypeConfig.Schema != null;
				if (flag2)
				{
					this.UpdateCategories();
				}
				bool flag3 = assetTypeConfig.SidebarButtons != null;
				if (flag3)
				{
					Document document;
					this.Desktop.Provider.TryGetDocument("AssetEditor/ConfigEditorContextPaneButton.ui", out document);
					using (List<AssetTypeConfig.Button>.Enumerator enumerator = assetTypeConfig.SidebarButtons.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AssetTypeConfig.Button sidebarButton = enumerator.Current;
							TextButton textButton = document.Instantiate(this.Desktop, this._buttonsGroup).Get<TextButton>("Button");
							textButton.Text = this._assetEditorOverlay.Backend.GetButtonText(sidebarButton.TextId ?? "");
							textButton.Activating = delegate()
							{
								this.SendAction(sidebarButton.Action);
							};
						}
					}
				}
				else
				{
					bool flag4 = assetTypeConfig.HasFeature(AssetTypeConfig.EditorFeature.WeatherDaytimeBar);
					if (flag4)
					{
						this._buttonsGroup.Add(this.DayTimeControls, -1);
					}
				}
			}
			this.UpdatePreview(false);
			base.Layout(null, true);
		}

		// Token: 0x06005E46 RID: 24134 RVA: 0x001E2060 File Offset: 0x001E0260
		public void UpdatePreview(bool doLayout = true)
		{
			AssetTypeConfig assetTypeConfig;
			bool flag = this._assetEditorOverlay.CurrentAsset.Type != null && this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(this._assetEditorOverlay.CurrentAsset.Type, out assetTypeConfig);
			if (flag)
			{
				bool flag2 = assetTypeConfig.Preview > AssetTypeConfig.PreviewType.None;
				if (flag2)
				{
					this._previewGroup.Anchor.Height = new int?((assetTypeConfig.Preview == AssetTypeConfig.PreviewType.Item) ? 280 : 380);
					this._previewGroup.Visible = true;
					AssetEditorAppEditor editor = this._assetEditorOverlay.Interface.App.Editor;
					bool flag3 = editor.ModelPreview != null;
					if (flag3)
					{
						this._modelPreview.Setup(editor.ModelPreview, editor.PreviewCameraSettings);
						this._modelPreview.Visible = true;
						this._blockPreview.Visible = false;
					}
					else
					{
						bool flag4 = editor.BlockPreview != null;
						if (flag4)
						{
							this._blockPreview.Setup(editor.BlockPreview, editor.PreviewCameraSettings);
							this._blockPreview.Visible = true;
							this._modelPreview.Visible = false;
						}
					}
				}
				else
				{
					this._previewGroup.Visible = false;
				}
			}
			else
			{
				this._previewGroup.Visible = false;
			}
			this._previewGroup.Anchor.Height = new int?(this._assetEditorOverlay.Interface.App.Settings.PaneSizes[AssetEditorSettings.Panes.ConfigEditorSidebarPreviewModel]);
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06005E47 RID: 24135 RVA: 0x001E220C File Offset: 0x001E040C
		public void UpdateCategories()
		{
			this._sectionButtons.Clear();
			this._sectionsGroup.Clear();
			this._activeCategory = this._assetEditorOverlay.ConfigEditor.State.ActiveCategory.ToString();
			bool flag = this._assetEditorOverlay.Mode == AssetEditorOverlay.EditorMode.Editor && this._assetEditorOverlay.ConfigEditor.Categories.Count > 1;
			if (flag)
			{
				ConfigEditorContextPane.<>c__DisplayClass18_0 CS$<>8__locals1;
				this.Desktop.Provider.TryGetDocument("AssetEditor/ConfigEditorContextPaneSectionButton.ui", out CS$<>8__locals1.doc);
				this.<UpdateCategories>g__MakeSectionButton|18_0("", this.Desktop.Provider.GetText("ui.assetEditor.configEditor.showAllProperties", null, true), ref CS$<>8__locals1);
				Group group = new Group(this.Desktop, this._sectionsGroup);
				group.Anchor = new Anchor
				{
					Height = new int?(1)
				};
				group.Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 50));
				foreach (KeyValuePair<string, string> keyValuePair in this._assetEditorOverlay.ConfigEditor.Categories)
				{
					this.<UpdateCategories>g__MakeSectionButton|18_0(keyValuePair.Key, keyValuePair.Value, ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x06005E48 RID: 24136 RVA: 0x001E2380 File Offset: 0x001E0580
		public void OnTrackedAssetChanged(TrackedAsset trackedAsset)
		{
			bool isMounted = this._modelPreview.IsMounted;
			if (isMounted)
			{
				this._modelPreview.OnTrackedAssetChanged(trackedAsset);
			}
			bool isMounted2 = this._blockPreview.IsMounted;
			if (isMounted2)
			{
				this._blockPreview.OnTrackedAssetChanged(trackedAsset);
			}
		}

		// Token: 0x06005E4A RID: 24138 RVA: 0x001E2478 File Offset: 0x001E0678
		[CompilerGenerated]
		private void <UpdateCategories>g__MakeSectionButton|18_0(string path, string text, ref ConfigEditorContextPane.<>c__DisplayClass18_0 A_3)
		{
			int num = path.Split(new char[]
			{
				'.'
			}).Length;
			UIFragment uifragment = A_3.doc.Instantiate(this.Desktop, this._sectionsGroup);
			TextButton textButton = uifragment.Get<TextButton>("Button");
			textButton.Padding.Left = new int?(textButton.Padding.Left.GetValueOrDefault() + (num - 1) * 16);
			textButton.Text = text;
			textButton.Activating = delegate()
			{
				this._assetEditorOverlay.ConfigEditor.State.ActiveCategory = new PropertyPath?(PropertyPath.FromString(path));
				this._assetEditorOverlay.ConfigEditor.Update();
				this._assetEditorOverlay.ConfigEditor.ScrollToTop();
				this.SetActiveCategory(path, true);
			};
			uifragment.Get<Group>("Icon").Visible = (num > 1);
			bool flag = path == this._activeCategory;
			if (flag)
			{
				textButton.Style = this._activeStyle;
			}
			this._sectionButtons[path] = textButton;
		}

		// Token: 0x04003AD9 RID: 15065
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003ADA RID: 15066
		public readonly DayTimeControls DayTimeControls;

		// Token: 0x04003ADB RID: 15067
		private DynamicPane _previewGroup;

		// Token: 0x04003ADC RID: 15068
		private ModelPreview _modelPreview;

		// Token: 0x04003ADD RID: 15069
		private BlockPreview _blockPreview;

		// Token: 0x04003ADE RID: 15070
		private Group _buttonsGroup;

		// Token: 0x04003ADF RID: 15071
		private Group _sectionsGroup;

		// Token: 0x04003AE0 RID: 15072
		private readonly Dictionary<string, TextButton> _sectionButtons = new Dictionary<string, TextButton>();

		// Token: 0x04003AE1 RID: 15073
		private TextButton.TextButtonStyle _style;

		// Token: 0x04003AE2 RID: 15074
		private TextButton.TextButtonStyle _activeStyle;

		// Token: 0x04003AE3 RID: 15075
		private string _activeCategory;
	}
}
