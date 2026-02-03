using System;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x02000881 RID: 2177
	internal class FloatingItemComponent : InterfaceComponent
	{
		// Token: 0x06003D92 RID: 15762 RVA: 0x0009F0A8 File Offset: 0x0009D2A8
		public FloatingItemComponent(InGameView inGameView, Element parent) : base(inGameView.Interface, parent)
		{
			this.InGameView = inGameView;
			this.Anchor = new Anchor
			{
				Width = new int?(this.InGameView.DefaultItemGridStyle.SlotIconSize),
				Height = new int?(this.InGameView.DefaultItemGridStyle.SlotIconSize)
			};
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0009F114 File Offset: 0x0009D314
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._dropIconTexturePatch = this.Desktop.MakeTexturePatch(new PatchStyle("InGame/Pages/Inventory/DropIcon.png"));
			this._unknownItemIconPatch = this.Desktop.MakeTexturePatch(new PatchStyle("InGame/Pages/Inventory/UnknownItemIcon.png"));
			this._quantityFont = this.Desktop.Provider.GetFontFamily("Default").RegularFont;
			this.Slot.ApplyStyles(this.InGameView, this.Desktop);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0009F198 File Offset: 0x0009D398
		protected override void LayoutSelf()
		{
			int num = this.Desktop.ScaleRound(32f);
			this._dropIconRectangle = new Rectangle(this._rectangleAfterPadding.X + this._rectangleAfterPadding.Width - num / 2, this._rectangleAfterPadding.Y - num / 2, num, num);
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this.Slot.ItemIcon != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawTexture(this.Slot.ItemIcon.Texture, this.Slot.ItemIcon.Rectangle, this._rectangleAfterPadding, UInt32Color.White);
			}
			else
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._unknownItemIconPatch, this._rectangleAfterPadding, this.Desktop.Scale);
			}
			bool showDropIcon = this.ShowDropIcon;
			if (showDropIcon)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._dropIconTexturePatch, this._dropIconRectangle, this.Desktop.Scale);
			}
			bool flag2 = this.Slot.ItemStack.Quantity > 1;
			if (flag2)
			{
				int num = this.Desktop.ScaleRound((float)((this.InGameView.DefaultItemGridStyle.SlotSize - this.InGameView.DefaultItemGridStyle.SlotIconSize) / 2));
				int num2 = this.Desktop.ScaleRound((float)this.InGameView.DefaultItemGridStyle.SlotIconSize);
				int num3 = this.Desktop.ScaleRound((float)this.InGameView.DefaultItemGridStyle.SlotSize);
				string text = this.Slot.ItemStack.Quantity.ToString();
				int num4 = this._rectangleAfterPadding.X + num2 - this.Desktop.ScaleRound(this._quantityFont.CalculateTextWidth(text) * 16f / (float)this._quantityFont.BaseSize);
				float y = (float)(this._rectangleAfterPadding.Y + num3 - num) - 26f * this.Desktop.Scale;
				this.Desktop.Batcher2D.RequestDrawText(this._quantityFont, 16f * this.Desktop.Scale, text, new Vector3((float)num4, y, 0f), UInt32Color.White, false, false, 0f);
			}
		}

		// Token: 0x04001CA8 RID: 7336
		private readonly InGameView InGameView;

		// Token: 0x04001CA9 RID: 7337
		public ItemGridSlot Slot;

		// Token: 0x04001CAA RID: 7338
		public bool ShowDropIcon;

		// Token: 0x04001CAB RID: 7339
		private Font _quantityFont;

		// Token: 0x04001CAC RID: 7340
		private TexturePatch _dropIconTexturePatch;

		// Token: 0x04001CAD RID: 7341
		private TexturePatch _unknownItemIconPatch;

		// Token: 0x04001CAE RID: 7342
		private Rectangle _dropIconRectangle;
	}
}
