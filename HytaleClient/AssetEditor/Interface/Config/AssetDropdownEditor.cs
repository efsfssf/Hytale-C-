using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BB5 RID: 2997
	internal class AssetDropdownEditor : ValueEditor
	{
		// Token: 0x06005DC9 RID: 24009 RVA: 0x001DE548 File Offset: 0x001DC748
		public AssetDropdownEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x001DE56C File Offset: 0x001DC76C
		protected override void Build()
		{
			this._layoutMode = LayoutMode.Left;
			string text = this.Schema.AssetType;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this.ConfigEditor.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(text, out assetTypeConfig);
			if (!flag)
			{
				bool flag2 = assetTypeConfig.IdProvider != null;
				if (flag2)
				{
					text = assetTypeConfig.IdProvider;
				}
				this._dropdown = new AssetSelectorDropdown(this.Desktop, this, this.ConfigEditor.AssetEditorOverlay)
				{
					FlexWeight = 1,
					AssetType = text,
					Value = (string)base.Value,
					ValueChanged = delegate()
					{
						base.HandleChangeValue(this._dropdown.Value, false, false, false);
						this.Validate();
					},
					Style = this.ConfigEditor.FileDropdownBoxStyle
				};
				Group group = new Group(this.Desktop, this);
				group.Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 70));
				group.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				Button button = new Button(this.Desktop, this);
				button.Anchor = new Anchor
				{
					Width = new int?(32)
				};
				button.TooltipText = (base.TooltipText = this.Desktop.Provider.GetText("ui.assetEditor.assetDropdownEditor.openAssetTooltip", null, true));
				button.Background = new PatchStyle("AssetEditor/OpenAssetIcon.png");
				button.Activating = new Action(this.OpenSelectedAssetInNewTab);
				Group group2 = new Group(this.Desktop, this);
				group2.Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 70));
				group2.Anchor = new Anchor
				{
					Width = new int?(1)
				};
				TextButton textButton = new TextButton(this.Desktop, this);
				textButton.Anchor = new Anchor
				{
					Width = new int?(30)
				};
				textButton.Style = new TextButton.TextButtonStyle
				{
					Default = new TextButton.TextButtonStyleState
					{
						LabelStyle = new LabelStyle
						{
							RenderBold = true,
							FontSize = 20f,
							Alignment = LabelStyle.LabelAlignment.Center
						}
					}
				};
				textButton.TooltipText = this.Desktop.Provider.GetText("ui.assetEditor.assetDropdownEditor.createAssetTooltip", new Dictionary<string, string>
				{
					{
						"assetType",
						assetTypeConfig.Name
					}
				}, true);
				textButton.Text = "+";
				textButton.Activating = delegate()
				{
					this.CreateNewAssetAndReference(null, null);
				};
			}
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x001DE7DC File Offset: 0x001DC9DC
		public void OpenSelectedAssetInNewTab()
		{
			bool flag = this._dropdown == null || this._dropdown.Value == null;
			if (!flag)
			{
				string assetType = this._dropdown.AssetType;
				string filePath;
				bool flag2 = !this.ConfigEditor.AssetEditorOverlay.Assets.TryGetPathForAssetId(assetType, this._dropdown.Value, out filePath, false);
				if (!flag2)
				{
					this.ConfigEditor.AssetEditorOverlay.OpenExistingAsset(new AssetReference(assetType, filePath), true);
				}
			}
		}

		// Token: 0x06005DCC RID: 24012 RVA: 0x001DE85C File Offset: 0x001DCA5C
		public void CreateNewAssetAndReference(string assetToCopy = null, string id = null)
		{
			bool flag = this._dropdown == null;
			if (!flag)
			{
				bool flag2 = id == null;
				if (flag2)
				{
					id = this.ConfigEditor.GetCurrentAssetId();
				}
				string assetType = this._dropdown.AssetType;
				this.ConfigEditor.AssetEditorOverlay.CreateAssetModal.Open(assetType, assetToCopy, null, id, null, delegate(string filePath, FormattedMessage error)
				{
					bool flag3 = error != null || !this.IsMounted;
					if (!flag3)
					{
						this.HandleChangeValue(this.ConfigEditor.AssetEditorOverlay.GetAssetIdFromReference(new AssetReference(assetType, filePath)), false, false, false);
						this.Validate();
					}
				});
			}
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x001DE8D8 File Offset: 0x001DCAD8
		public void CopyAssetAndReference()
		{
			bool flag = this._dropdown == null || this._dropdown.Value == null;
			if (!flag)
			{
				string assetToCopy;
				bool flag2 = !this.ConfigEditor.AssetEditorOverlay.Assets.TryGetPathForAssetId(this._dropdown.AssetType, this._dropdown.Value, out assetToCopy, false);
				if (!flag2)
				{
					string currentAssetId = this.ConfigEditor.GetCurrentAssetId();
					this.CreateNewAssetAndReference(assetToCopy, currentAssetId);
				}
			}
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x001DE954 File Offset: 0x001DCB54
		public override void Focus()
		{
			bool flag = this._dropdown == null;
			if (!flag)
			{
				this._dropdown.Open();
			}
		}

		// Token: 0x06005DCF RID: 24015 RVA: 0x001DE980 File Offset: 0x001DCB80
		protected internal override void UpdateDisplayedValue()
		{
			bool flag = this._dropdown == null;
			if (!flag)
			{
				this._dropdown.Value = (string)base.Value;
				this._dropdown.Layout(null, true);
			}
		}

		// Token: 0x06005DD0 RID: 24016 RVA: 0x001DE9CA File Offset: 0x001DCBCA
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8;
		}

		// Token: 0x04003AAB RID: 15019
		private AssetSelectorDropdown _dropdown;
	}
}
