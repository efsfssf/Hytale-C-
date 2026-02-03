using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086B RID: 2155
	internal class OpacitySelector : Element
	{
		// Token: 0x06003CC5 RID: 15557 RVA: 0x00098BE8 File Offset: 0x00096DE8
		public OpacitySelector(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._label = new Label(this.Desktop, this)
			{
				Visible = false,
				Background = new PatchStyle(UInt32Color.FromRGBA(0, 0, 0, 150)),
				Style = new LabelStyle
				{
					FontSize = 13f,
					Alignment = LabelStyle.LabelAlignment.Center
				},
				Anchor = new Anchor
				{
					Width = new int?(38),
					Height = new int?(24),
					Top = new int?(-30)
				}
			};
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00098C89 File Offset: 0x00096E89
		protected override void OnUnmounted()
		{
			this._label.Visible = false;
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x00098C9C File Offset: 0x00096E9C
		protected override void ApplyStyles()
		{
			this.Background = this.Style.OpacitySelectorBackground;
			base.ApplyStyles();
			this._buttonPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonBackground);
			this._buttonFillPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonFill);
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00098CFC File Offset: 0x00096EFC
		protected override void OnMouseEnter()
		{
			this._label.Visible = true;
			base.Layout(null, true);
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00098D28 File Offset: 0x00096F28
		protected override void OnMouseLeave()
		{
			this._label.Visible = false;
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00098D38 File Offset: 0x00096F38
		protected override void OnMouseMove()
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateOpacityFromMousePosition();
			}
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00098D98 File Offset: 0x00096F98
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateOpacityFromMousePosition();
			}
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00098DF5 File Offset: 0x00096FF5
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			Action mouseButtonReleased = this.MouseButtonReleased;
			if (mouseButtonReleased != null)
			{
				mouseButtonReleased();
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00098E0A File Offset: 0x0009700A
		protected override void LayoutSelf()
		{
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x00098E14 File Offset: 0x00097014
		public override Element HitTest(Point position)
		{
			bool flag = !base.Visible || (!this._rectangleAfterPadding.Contains(position) && !this._buttonRectangle.Contains(position));
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x00098E5C File Offset: 0x0009705C
		private void UpdateButtonRectangle()
		{
			int num = this.Desktop.ScaleRound(16f);
			int width = this._rectangleAfterPadding.Width;
			float num2 = (float)this._rectangleAfterPadding.X + this.Opacity * (float)width - (float)num / 2f;
			int y = this._rectangleAfterPadding.Y + this._rectangleAfterPadding.Height / 2 - num / 2;
			this._buttonRectangle = new Rectangle((int)num2, y, num, num);
			this._label.Text = this.Desktop.Provider.FormatNumber((int)(this.Opacity * 100f)) + "%";
			this._label.Anchor.Left = this.Desktop.UnscaleRound(this.Opacity * (float)width) - this._label.Anchor.Width / 2;
			this._label.Layout(null, true);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x00098FA0 File Offset: 0x000971A0
		private void UpdateOpacityFromMousePosition()
		{
			this.Opacity = MathHelper.Clamp((float)(this.Desktop.MousePosition.X - this._rectangleAfterPadding.Left) / (float)this._rectangleAfterPadding.Width, 0f, 1f);
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00098FF4 File Offset: 0x000971F4
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			byte b = (byte)(255f * (1f - this.Opacity));
			this.Desktop.Batcher2D.RequestDrawTexture(this._buttonFillPatch.TextureArea.Texture, this._buttonFillPatch.TextureArea.Rectangle, this._buttonRectangle, UInt32Color.FromRGBA(b, b, b, byte.MaxValue));
			this.Desktop.Batcher2D.RequestDrawPatch(this._buttonPatch, this._buttonRectangle, this.Desktop.Scale);
		}

		// Token: 0x04001C33 RID: 7219
		public float Opacity;

		// Token: 0x04001C34 RID: 7220
		public ColorPickerStyle Style;

		// Token: 0x04001C35 RID: 7221
		private TexturePatch _buttonPatch;

		// Token: 0x04001C36 RID: 7222
		private TexturePatch _buttonFillPatch;

		// Token: 0x04001C37 RID: 7223
		public Action MouseButtonReleased;

		// Token: 0x04001C38 RID: 7224
		private Rectangle _buttonRectangle;

		// Token: 0x04001C39 RID: 7225
		private readonly Label _label;
	}
}
