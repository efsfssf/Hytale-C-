using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000863 RID: 2147
	internal class HueSelector : Element
	{
		// Token: 0x06003C37 RID: 15415 RVA: 0x00092B00 File Offset: 0x00090D00
		public HueSelector(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x00092B0C File Offset: 0x00090D0C
		protected override void OnMounted()
		{
			this._textureRect = new Rectangle(0, 0, 1, 400);
			byte[] array = new byte[this._textureRect.Height * 4];
			for (int i = 0; i < this._textureRect.Height; i++)
			{
				int num = i * 4;
				ColorHsva colorHsva = new ColorHsva((float)(this._textureRect.Height - i) / (float)this._textureRect.Height, 1f, 1f, 1f);
				byte b;
				byte b2;
				byte b3;
				byte b4;
				colorHsva.ToRgba(out b, out b2, out b3, out b4);
				array[num] = b;
				array[num + 1] = b2;
				array[num + 2] = b3;
				array[num + 3] = byte.MaxValue;
			}
			this._texture = new Texture(Texture.TextureTypes.Texture2D);
			this._texture.CreateTexture2D(this._textureRect.Width, this._textureRect.Height, array, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x00092C14 File Offset: 0x00090E14
		protected override void OnUnmounted()
		{
			this._texture.Dispose();
			this._texture = null;
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x00092C2C File Offset: 0x00090E2C
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._buttonPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonBackground);
			this._buttonFillPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonFill);
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x00092C7C File Offset: 0x00090E7C
		protected override void OnMouseMove()
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateHueFromMousePosition();
			}
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x00092CDC File Offset: 0x00090EDC
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateHueFromMousePosition();
			}
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x00092D39 File Offset: 0x00090F39
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			Action mouseButtonReleased = this.MouseButtonReleased;
			if (mouseButtonReleased != null)
			{
				mouseButtonReleased();
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x00092D4E File Offset: 0x00090F4E
		protected override void LayoutSelf()
		{
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x00092D58 File Offset: 0x00090F58
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

		// Token: 0x06003C40 RID: 15424 RVA: 0x00092DA0 File Offset: 0x00090FA0
		private void UpdateButtonRectangle()
		{
			int num = this.Desktop.ScaleRound(16f);
			int height = this._rectangleAfterPadding.Height;
			int num2 = Math.Min((int)Math.Round((double)((float)height - this.Hue * (float)height)), height - 1);
			int x = this._rectangleAfterPadding.X + this._rectangleAfterPadding.Width / 2 - num / 2;
			float num3 = (float)(this._rectangleAfterPadding.Y + num2) - (float)num / 2f;
			this._buttonRectangle = new Rectangle(x, (int)num3, num, num);
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x00092E30 File Offset: 0x00091030
		private void UpdateHueFromMousePosition()
		{
			this.Hue = 1f - MathHelper.Clamp((float)(this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Top) / (float)this._rectangleAfterPadding.Height, 0f, 1f);
			Action<float> valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged(this.Hue);
			}
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x00092EA4 File Offset: 0x000910A4
		protected override void PrepareForDrawSelf()
		{
			this.Desktop.Batcher2D.RequestDrawTexture(this._texture, this._textureRect, this._rectangleAfterPadding, UInt32Color.White);
			ColorHsva colorHsva = new ColorHsva(this.Hue, 1f, 1f, 1f);
			byte r;
			byte g;
			byte b;
			byte b2;
			colorHsva.ToRgba(out r, out g, out b, out b2);
			this.Desktop.Batcher2D.RequestDrawTexture(this._buttonFillPatch.TextureArea.Texture, this._buttonFillPatch.TextureArea.Rectangle, this._buttonRectangle, UInt32Color.FromRGBA(r, g, b, byte.MaxValue));
			this.Desktop.Batcher2D.RequestDrawPatch(this._buttonPatch, this._buttonRectangle, this.Desktop.Scale);
		}

		// Token: 0x04001BD2 RID: 7122
		public float Hue;

		// Token: 0x04001BD3 RID: 7123
		public ColorPickerStyle Style;

		// Token: 0x04001BD4 RID: 7124
		private TexturePatch _buttonPatch;

		// Token: 0x04001BD5 RID: 7125
		private TexturePatch _buttonFillPatch;

		// Token: 0x04001BD6 RID: 7126
		public Action<float> ValueChanged;

		// Token: 0x04001BD7 RID: 7127
		public Action MouseButtonReleased;

		// Token: 0x04001BD8 RID: 7128
		private Rectangle _buttonRectangle;

		// Token: 0x04001BD9 RID: 7129
		private Texture _texture;

		// Token: 0x04001BDA RID: 7130
		private Rectangle _textureRect;
	}
}
