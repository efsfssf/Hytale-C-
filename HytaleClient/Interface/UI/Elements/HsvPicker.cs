using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000862 RID: 2146
	internal class HsvPicker : Element
	{
		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06003C20 RID: 15392 RVA: 0x000923D8 File Offset: 0x000905D8
		// (set) Token: 0x06003C21 RID: 15393 RVA: 0x000923E0 File Offset: 0x000905E0
		public float Hue { get; private set; }

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x000923E9 File Offset: 0x000905E9
		// (set) Token: 0x06003C23 RID: 15395 RVA: 0x000923F1 File Offset: 0x000905F1
		public float Saturation { get; private set; }

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x000923FA File Offset: 0x000905FA
		// (set) Token: 0x06003C25 RID: 15397 RVA: 0x00092402 File Offset: 0x00090602
		public float Value { get; private set; }

		// Token: 0x06003C26 RID: 15398 RVA: 0x0009240B File Offset: 0x0009060B
		public HsvPicker(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x00092417 File Offset: 0x00090617
		protected override void ApplyStyles()
		{
			this._buttonPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonBackground);
			this._buttonFillPatch = this.Desktop.MakeTexturePatch(this.Style.ButtonFill);
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x00092454 File Offset: 0x00090654
		protected override void OnMounted()
		{
			this._textureRect = new Rectangle(0, 0, 400, 400);
			this._data = new byte[this._textureRect.Width * this._textureRect.Height * 4];
			this._texture = new Texture(Texture.TextureTypes.Texture2D);
			this._texture.CreateTexture2D(this._textureRect.Width, this._textureRect.Height, this._data, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._isTextureDirty = true;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x00092515 File Offset: 0x00090715
		protected override void OnUnmounted()
		{
			this._texture.Dispose();
			this._texture = null;
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x00092544 File Offset: 0x00090744
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateColorFromMousePosition();
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x000925B4 File Offset: 0x000907B4
		protected override void OnMouseMove()
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = !(num.GetValueOrDefault() == num2 & num != null);
			if (!flag)
			{
				this.UpdateColorFromMousePosition();
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x00092623 File Offset: 0x00090823
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			Action mouseButtonReleased = this.MouseButtonReleased;
			if (mouseButtonReleased != null)
			{
				mouseButtonReleased();
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x00092638 File Offset: 0x00090838
		protected override void LayoutSelf()
		{
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x00092644 File Offset: 0x00090844
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

		// Token: 0x06003C2F RID: 15407 RVA: 0x0009268C File Offset: 0x0009088C
		private void UpdateButtonRectangle()
		{
			int num = this.Desktop.ScaleRound(16f);
			int width = this._rectangleAfterPadding.Width;
			int height = this._rectangleAfterPadding.Height;
			int num2 = Math.Min((int)Math.Round((double)(this.Saturation * (float)width)), width - 1);
			int num3 = Math.Min((int)Math.Round((double)(this.Value * (float)height)), height - 1);
			float num4 = (float)(this._rectangleAfterPadding.X + num2) - (float)num / 2f;
			float num5 = (float)(this._rectangleAfterPadding.Y + (height - num3)) - (float)num / 2f;
			this._buttonRectangle = new Rectangle((int)num4, (int)num5, num, num);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x00092740 File Offset: 0x00090940
		private void UpdateColorFromMousePosition()
		{
			this.Saturation = MathHelper.Clamp((float)(this.Desktop.MousePosition.X - this._rectangleAfterPadding.Left) / (float)this._rectangleAfterPadding.Width, 0f, 1f);
			this.Value = 1f - MathHelper.Clamp((float)(this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Top) / (float)this._rectangleAfterPadding.Height, 0f, 1f);
			this._isTextureDirty = true;
			this.UpdateButtonRectangle();
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x000927E4 File Offset: 0x000909E4
		private void Animate(float dt)
		{
			bool flag = !this._isTextureDirty;
			if (!flag)
			{
				this._isTextureDirty = false;
				this.UpdateTexture();
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0009280F File Offset: 0x00090A0F
		public void SetHue(float hue)
		{
			this.Hue = hue;
			this._isTextureDirty = true;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x00092821 File Offset: 0x00090A21
		public void SetColor(float hue, float saturation, float value)
		{
			this.Hue = hue;
			this.Saturation = saturation;
			this.Value = value;
			this._isTextureDirty = true;
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x00092844 File Offset: 0x00090A44
		public ColorRgba GetColorRgba()
		{
			ColorHsva colorHsva = new ColorHsva(this.Hue, this.Saturation, this.Value, 1f);
			byte r;
			byte g;
			byte b;
			byte b2;
			colorHsva.ToRgba(out r, out g, out b, out b2);
			return new ColorRgba(r, g, b, byte.MaxValue);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x00092894 File Offset: 0x00090A94
		private void UpdateTexture()
		{
			int width = this._texture.Width;
			int height = this._texture.Height;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					ColorHsva colorHsva = new ColorHsva(this.Hue, (float)j / (float)width, (float)i / (float)height, 1f);
					byte b;
					byte b2;
					byte b3;
					byte b4;
					colorHsva.ToRgba(out b, out b2, out b3, out b4);
					int num = ((height - i - 1) * width + j) * 4;
					bool isShortColor = this.IsShortColor;
					if (isShortColor)
					{
						this._data[num] = (byte)(Math.Round((double)((float)b / 255f * 15f)) / 15.0 * 255.0);
						this._data[num + 1] = (byte)(Math.Round((double)((float)b2 / 255f * 15f)) / 15.0 * 255.0);
						this._data[num + 2] = (byte)(Math.Round((double)((float)b3 / 255f * 15f)) / 15.0 * 255.0);
						this._data[num + 3] = byte.MaxValue;
					}
					else
					{
						this._data[num] = b;
						this._data[num + 1] = b2;
						this._data[num + 2] = b3;
						this._data[num + 3] = byte.MaxValue;
					}
				}
			}
			this._texture.UpdateTexture2D(this._data);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x00092A30 File Offset: 0x00090C30
		protected override void PrepareForDrawSelf()
		{
			this.Desktop.Batcher2D.RequestDrawTexture(this._texture, this._textureRect, this._rectangleAfterPadding, UInt32Color.White);
			ColorHsva colorHsva = new ColorHsva(this.Hue, this.Saturation, this.Value, 1f);
			byte r;
			byte g;
			byte b;
			byte b2;
			colorHsva.ToRgba(out r, out g, out b, out b2);
			this.Desktop.Batcher2D.RequestDrawTexture(this._buttonFillPatch.TextureArea.Texture, this._buttonFillPatch.TextureArea.Rectangle, this._buttonRectangle, UInt32Color.FromRGBA(r, g, b, byte.MaxValue));
			this.Desktop.Batcher2D.RequestDrawPatch(this._buttonPatch, this._buttonRectangle, this.Desktop.Scale);
		}

		// Token: 0x04001BC4 RID: 7108
		private Texture _texture;

		// Token: 0x04001BC5 RID: 7109
		private Rectangle _textureRect;

		// Token: 0x04001BC6 RID: 7110
		private byte[] _data;

		// Token: 0x04001BC7 RID: 7111
		private bool _isTextureDirty;

		// Token: 0x04001BCB RID: 7115
		public ColorPickerStyle Style;

		// Token: 0x04001BCC RID: 7116
		private TexturePatch _buttonPatch;

		// Token: 0x04001BCD RID: 7117
		private TexturePatch _buttonFillPatch;

		// Token: 0x04001BCE RID: 7118
		private Rectangle _buttonRectangle;

		// Token: 0x04001BCF RID: 7119
		public Action ValueChanged;

		// Token: 0x04001BD0 RID: 7120
		public Action MouseButtonReleased;

		// Token: 0x04001BD1 RID: 7121
		public bool IsShortColor;
	}
}
