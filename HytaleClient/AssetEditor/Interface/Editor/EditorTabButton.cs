using System;
using System.Collections.Generic;
using System.IO;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Editor
{
	// Token: 0x02000BB1 RID: 2993
	internal class EditorTabButton : Button
	{
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06005D97 RID: 23959 RVA: 0x001DCE55 File Offset: 0x001DB055
		// (set) Token: 0x06005D98 RID: 23960 RVA: 0x001DCE5D File Offset: 0x001DB05D
		public bool IsActive { get; private set; }

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06005D99 RID: 23961 RVA: 0x001DCE66 File Offset: 0x001DB066
		// (set) Token: 0x06005D9A RID: 23962 RVA: 0x001DCE6E File Offset: 0x001DB06E
		public DateTime TimeLastActive { get; private set; }

		// Token: 0x06005D9B RID: 23963 RVA: 0x001DCE78 File Offset: 0x001DB078
		public EditorTabButton(AssetEditorOverlay assetEditorOverlay, AssetReference assetReference) : base(assetEditorOverlay.Desktop, null)
		{
			this.AssetReference = assetReference;
			this._assetEditorOverlay = assetEditorOverlay;
			this._path = ((this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type].AssetTree == AssetTreeFolder.Cosmetics) ? AssetPathUtils.GetPathWithoutAssetId(this.AssetReference.FilePath) : Path.GetDirectoryName(this.AssetReference.FilePath));
			this._name = this._assetEditorOverlay.GetAssetIdFromReference(this.AssetReference);
		}

		// Token: 0x06005D9C RID: 23964 RVA: 0x001DCF04 File Offset: 0x001DB104
		public void Build()
		{
			base.Clear();
			AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[this.AssetReference.Type];
			base.TooltipText = assetTypeConfig.Name;
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/TabButton.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			Group group = uifragment.Get<Group>("Icon");
			group.Background = assetTypeConfig.Icon.Clone();
			group.Background.Color = (assetTypeConfig.IsColoredIcon ? UInt32Color.White : UInt32Color.FromRGBA(160, 160, 160, byte.MaxValue));
			this._pathLabel = uifragment.Get<Label>("Directory");
			this._pathLabel.Text = this._path;
			this._nameLabel = uifragment.Get<Label>("Name");
			this._nameLabel.Text = this._name;
			this._closeButton = uifragment.Get<Button>("CloseButton");
			this._closeButton.Activating = delegate()
			{
				this._assetEditorOverlay.CloseTab(this.AssetReference);
			};
			this._activeHighlight = uifragment.Get<Group>("ActiveTabHighlight");
		}

		// Token: 0x06005D9D RID: 23965 RVA: 0x001DD03C File Offset: 0x001DB23C
		protected override void OnMouseEnter()
		{
			base.OnMouseEnter();
			this._closeButton.Disabled = false;
			this._closeButton.Layout(null, true);
		}

		// Token: 0x06005D9E RID: 23966 RVA: 0x001DD074 File Offset: 0x001DB274
		protected override void OnMouseLeave()
		{
			base.OnMouseLeave();
			this._closeButton.Disabled = true;
			this._closeButton.Layout(null, true);
		}

		// Token: 0x06005D9F RID: 23967 RVA: 0x001DD0AC File Offset: 0x001DB2AC
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			FontFamily fontFamily = this.Desktop.Provider.GetFontFamily(this._nameLabel.Style.FontName.Value);
			Font font = this._nameLabel.Style.RenderBold ? fontFamily.BoldFont : fontFamily.RegularFont;
			float num = font.CalculateTextWidth(this._name) * this._nameLabel.Style.FontSize / (float)font.BaseSize;
			this._pathLabel.Anchor.Width = new int?(Math.Max((int)num, 150));
			return base.ComputeScaledMinSize(maxWidth, maxHeight);
		}

		// Token: 0x06005DA0 RID: 23968 RVA: 0x001DD158 File Offset: 0x001DB358
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			FontFamily fontFamily = this.Desktop.Provider.GetFontFamily(this._pathLabel.Style.FontName.Value);
			Font font = this._nameLabel.Style.RenderBold ? fontFamily.BoldFont : fontFamily.RegularFont;
			string str = "";
			float num = (float)this._pathLabel.Anchor.Width.Value - font.GetCharacterAdvance(8230) * this._pathLabel.Style.FontSize / (float)font.BaseSize;
			for (int i = this._path.Length - 1; i >= 0; i--)
			{
				float num2 = font.GetCharacterAdvance((ushort)this._path[i]) * this._pathLabel.Style.FontSize / (float)font.BaseSize;
				num -= num2;
				bool flag = num <= 0f;
				if (flag)
				{
					break;
				}
				str = this._path[i].ToString() + str;
				num -= this._pathLabel.Style.LetterSpacing;
			}
			this._pathLabel.Text = "…" + str;
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x001DD2B4 File Offset: 0x001DB4B4
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			base.OnMouseButtonDown(evt);
			bool flag = !this.Disabled;
			if (flag)
			{
				uint button = (uint)evt.Button;
				uint num = button;
				if (num != 2U)
				{
					if (num == 3U)
					{
						this.OpenContextPopup();
					}
				}
				else
				{
					this._assetEditorOverlay.CloseTab(AssetReference.None);
				}
			}
		}

		// Token: 0x06005DA2 RID: 23970 RVA: 0x001DD30C File Offset: 0x001DB50C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = !this.Disabled && activate && (long)evt.Button == 1L;
			if (flag)
			{
				this._assetEditorOverlay.OpenExistingAsset(this.AssetReference, evt.Clicks == 2);
			}
		}

		// Token: 0x06005DA3 RID: 23971 RVA: 0x001DD360 File Offset: 0x001DB560
		public void SetActive(bool active)
		{
			if (active)
			{
				this.TimeLastActive = DateTime.Now;
			}
			bool flag = active == this.IsActive;
			if (!flag)
			{
				this.IsActive = active;
				this.Background = (this.IsActive ? new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 150)) : null);
				this._nameLabel.Style.TextColor = UInt32Color.FromHexString(this.IsActive ? "#111111" : "#575656");
				this._activeHighlight.Visible = this.IsActive;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06005DA4 RID: 23972 RVA: 0x001DD420 File Offset: 0x001DB620
		private void OpenContextPopup()
		{
			PopupMenuLayer popup = this._assetEditorOverlay.Popup;
			List<PopupMenuItem> items = new List<PopupMenuItem>
			{
				new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.tabs.popup.close", null, true), delegate()
				{
					this._assetEditorOverlay.CloseTab(this.AssetReference);
				}, null, null),
				new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.tabs.popup.closeAll", null, true), delegate()
				{
					this._assetEditorOverlay.CloseAllTabs();
				}, null, null),
				new PopupMenuItem(this.Desktop.Provider.GetText("ui.assetEditor.tabs.popup.copyId", null, true), delegate()
				{
					SDL.SDL_SetClipboardText(this._name);
				}, null, null)
			};
			this._assetEditorOverlay.SetupAssetPopup(this.AssetReference, items);
			popup.SetTitle(null);
			popup.SetItems(items);
			popup.Open();
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x001DD500 File Offset: 0x001DB700
		public void OnAssetRenamed(AssetReference assetReference)
		{
			this.AssetReference = assetReference;
			this._path = ((this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type].AssetTree == AssetTreeFolder.Cosmetics) ? AssetPathUtils.GetPathWithoutAssetId(this.AssetReference.FilePath) : Path.GetDirectoryName(this.AssetReference.FilePath));
			this._name = this._assetEditorOverlay.GetAssetIdFromReference(this.AssetReference);
			this._pathLabel.Text = this._path;
			this._nameLabel.Text = this._name;
		}

		// Token: 0x04003A87 RID: 14983
		public AssetReference AssetReference;

		// Token: 0x04003A88 RID: 14984
		private string _path;

		// Token: 0x04003A89 RID: 14985
		private string _name;

		// Token: 0x04003A8A RID: 14986
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003A8B RID: 14987
		private Button _closeButton;

		// Token: 0x04003A8C RID: 14988
		private Label _nameLabel;

		// Token: 0x04003A8D RID: 14989
		private Label _pathLabel;

		// Token: 0x04003A8E RID: 14990
		private Group _activeHighlight;
	}
}
