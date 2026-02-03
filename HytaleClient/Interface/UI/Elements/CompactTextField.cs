using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085A RID: 2138
	[UIMarkupElement]
	public class CompactTextField : TextField
	{
		// Token: 0x06003B66 RID: 15206 RVA: 0x0008C401 File Offset: 0x0008A601
		public CompactTextField(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0008C414 File Offset: 0x0008A614
		protected override void Animate(float deltaTime)
		{
			base.Animate(deltaTime);
			bool flag;
			if (this._targetWidth != -1)
			{
				int? width = this.Anchor.Width;
				int targetWidth = this._targetWidth;
				flag = !(width.GetValueOrDefault() == targetWidth & width != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.Anchor.Width = new int?((int)MathHelper.Lerp((float)this.Anchor.Width.Value, (float)this._targetWidth, Math.Min(deltaTime * 16f, 1f)));
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0008C4B8 File Offset: 0x0008A6B8
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			bool flag = this._targetWidth == -1;
			this._targetWidth = ((this._isFocused || this.Value != "") ? this.ExpandedWidth : this.CollapsedWidth);
			bool flag2 = flag;
			if (flag2)
			{
				this.Anchor.Width = new int?(this._targetWidth);
			}
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0008C524 File Offset: 0x0008A724
		protected override void OnMouseEnter()
		{
			SDL.SDL_SetCursor(this._isFocused ? this.Desktop.Cursors.IBeam : this.Desktop.Cursors.Hand);
			base.Layout(null, true);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0008C574 File Offset: 0x0008A774
		protected override void OnMouseLeave()
		{
			base.OnMouseLeave();
			base.Layout(null, true);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0008C59C File Offset: 0x0008A79C
		protected internal override void OnFocus()
		{
			base.OnFocus();
			SDL.SDL_SetCursor(this.Desktop.Cursors.IBeam);
			bool flag = this.Value == "" && this.ExpandSound != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this.ExpandSound);
			}
			base.Layout(null, true);
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0008C614 File Offset: 0x0008A814
		protected internal override void OnBlur()
		{
			base.OnBlur();
			bool flag = this.Value == "" && this.CollapseSound != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this.CollapseSound);
			}
			base.Layout(null, true);
		}

		// Token: 0x04001B5D RID: 7005
		private int _targetWidth = -1;

		// Token: 0x04001B5E RID: 7006
		[UIMarkupProperty]
		public int CollapsedWidth;

		// Token: 0x04001B5F RID: 7007
		[UIMarkupProperty]
		public int ExpandedWidth;

		// Token: 0x04001B60 RID: 7008
		[UIMarkupProperty]
		public SoundStyle ExpandSound;

		// Token: 0x04001B61 RID: 7009
		[UIMarkupProperty]
		public SoundStyle CollapseSound;
	}
}
