using System;
using HytaleClient.Application;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BD RID: 2237
	internal abstract class ItemSlotSelector : BaseItemSlotSelector
	{
		// Token: 0x060040E5 RID: 16613 RVA: 0x000BCC48 File Offset: 0x000BAE48
		protected ItemSlotSelector(InGameView inGameView, bool enableEmptySlot = true) : base(inGameView, inGameView.HudContainer, enableEmptySlot)
		{
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x000BCC5C File Offset: 0x000BAE5C
		public virtual void Build()
		{
			string path = "InGame/Hud/ItemSlotSelector.ui";
			Document document;
			this.Interface.TryGetDocument(path, out document);
			this._pointerBackgroundTexture = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "PointerBackground");
			this._pointerBackgroundAnchor = document.ResolveNamedValue<Anchor>(this.Desktop.Provider, "PointerBackgroundAnchor");
			UIFragment uifragment = base.Build(document);
			this._itemNameLabel = uifragment.Get<Label>("ItemName");
			this._emptyIcon = uifragment.Get<Group>("EmptyIcon");
			this._emptyIcon.Visible = this._enableEmptySlot;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x000BCCF3 File Offset: 0x000BAEF3
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._pointerBackgroundPatch = this.Desktop.MakeTexturePatch(this._pointerBackgroundTexture);
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x000BCD14 File Offset: 0x000BAF14
		protected override void LayoutSelf()
		{
			base.LayoutSelf();
			int num = this.Desktop.ScaleRound((float)this._pointerBackgroundAnchor.Width.GetValueOrDefault());
			int height = this.Desktop.ScaleRound((float)this._pointerBackgroundAnchor.Height.GetValueOrDefault());
			float num2 = (float)this._rectangleAfterPadding.Left + (float)this._rectangleAfterPadding.Width / 2f - (float)num / 2f;
			float num3 = (float)this._rectangleAfterPadding.Top + (float)this._rectangleAfterPadding.Height / 2f + (float)this.Desktop.ScaleRound((float)this._pointerBackgroundAnchor.Top.Value);
			this._pointerBackgroundRectangle = new Rectangle((int)num2, (int)num3, num, height);
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x000BCDE0 File Offset: 0x000BAFE0
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = activate && (long)evt.Button == 1L;
			if (flag)
			{
				this._doneChange = true;
				this.OnSlotSelected(this._enableEmptySlot ? (this._hoveredSlot - 1) : this._hoveredSlot, true);
				this._inGameView.InGame.SetActiveItemSelector(AppInGame.ItemSelector.None);
			}
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000BCE3E File Offset: 0x000BB03E
		protected override void OnSlotHovered()
		{
			this.UpdateItemNameLabel();
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x000BCE47 File Offset: 0x000BB047
		protected override void ItemStacksChanged()
		{
			this.UpdateItemNameLabel();
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x000BCE50 File Offset: 0x000BB050
		private void UpdateItemNameLabel()
		{
			bool flag = this._enableEmptySlot && this._hoveredSlot == 0;
			if (flag)
			{
				this._itemNameLabel.Text = this.Desktop.Provider.GetText("ui.hud.itemSlotSelector.noItem", null, true);
			}
			else
			{
				int num = this._enableEmptySlot ? (this._hoveredSlot - 1) : this._hoveredSlot;
				ClientItemStack clientItemStack = (num >= this._itemStacks.Length) ? null : this._itemStacks[num];
				this._itemNameLabel.Text = ((clientItemStack == null) ? "" : this.Desktop.Provider.GetText("items." + clientItemStack.Id + ".name", null, true));
			}
			this._itemNameLabel.Layout(null, true);
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x000BCF24 File Offset: 0x000BB124
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			Matrix transformationMatrix;
			BaseItemSlotSelector.CreateRotationMatrixAroundPoint(this._pointerBackgroundRectangle.X, this._rectangleAfterPadding.Y + this._rectangleAfterPadding.Height / 2, this._pointerBackgroundRectangle.Width, this._mousePointerAngle - 90f, out transformationMatrix);
			this.Desktop.Batcher2D.SetTransformationMatrix(transformationMatrix);
			this.Desktop.Batcher2D.RequestDrawPatch(this._pointerBackgroundPatch, this._pointerBackgroundRectangle, this.Desktop.Scale);
			this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
		}

		// Token: 0x04001F22 RID: 7970
		private Label _itemNameLabel;

		// Token: 0x04001F23 RID: 7971
		private PatchStyle _pointerBackgroundTexture;

		// Token: 0x04001F24 RID: 7972
		private Anchor _pointerBackgroundAnchor;

		// Token: 0x04001F25 RID: 7973
		private TexturePatch _pointerBackgroundPatch;

		// Token: 0x04001F26 RID: 7974
		private Rectangle _pointerBackgroundRectangle;

		// Token: 0x04001F27 RID: 7975
		private Group _emptyIcon;
	}
}
