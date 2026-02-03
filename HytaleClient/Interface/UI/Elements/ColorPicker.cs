using System;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000857 RID: 2135
	[UIMarkupElement]
	public class ColorPicker : InputElement<UInt32Color>
	{
		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x0008B2B0 File Offset: 0x000894B0
		// (set) Token: 0x06003B38 RID: 15160 RVA: 0x0008B311 File Offset: 0x00089511
		public override UInt32Color Value
		{
			get
			{
				ColorHsva colorHsva = new ColorHsva(this._hsvPicker.Hue, this._hsvPicker.Saturation, this._hsvPicker.Value, this._opacitySelector.Opacity);
				byte r;
				byte g;
				byte b;
				byte a;
				colorHsva.ToRgba(out r, out g, out b, out a);
				return UInt32Color.FromRGBA(r, g, b, a);
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x0008B31C File Offset: 0x0008951C
		// (set) Token: 0x06003B3A RID: 15162 RVA: 0x0008B324 File Offset: 0x00089524
		[UIMarkupProperty]
		public ColorPicker.ColorFormat Format
		{
			get
			{
				return this._colorFormat;
			}
			set
			{
				this._colorFormat = value;
				this._hsvPicker.IsShortColor = (value == ColorPicker.ColorFormat.RgbShort);
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06003B3B RID: 15163 RVA: 0x0008B33D File Offset: 0x0008953D
		// (set) Token: 0x06003B3C RID: 15164 RVA: 0x0008B34A File Offset: 0x0008954A
		[UIMarkupProperty]
		public bool DisplayTextField
		{
			get
			{
				return this._textField.Visible;
			}
			set
			{
				this._textField.Visible = value;
			}
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x0008B35C File Offset: 0x0008955C
		public ColorPicker(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._layoutMode = LayoutMode.Top;
			Group parent2 = new Group(this.Desktop, this)
			{
				LayoutMode = LayoutMode.Left,
				FlexWeight = 1
			};
			this._hsvPicker = new HsvPicker(desktop, parent2)
			{
				FlexWeight = 1,
				ValueChanged = delegate()
				{
					bool flag = this.ResetTransparencyWhenChangingColor && this.Format == ColorPicker.ColorFormat.Rgba && (double)Math.Abs(this._opacitySelector.Opacity) < 0.0001;
					if (flag)
					{
						this._opacitySelector.Opacity = 1f;
						this._opacitySelector.Layout(null, true);
					}
				},
				MouseButtonReleased = delegate()
				{
					this.UpdateTextField(this.Value);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				},
				IsShortColor = (this._colorFormat == ColorPicker.ColorFormat.RgbShort)
			};
			new Group(desktop, parent2).Anchor = new Anchor
			{
				Width = new int?(10)
			};
			this._hueSelector = new HueSelector(desktop, parent2)
			{
				Anchor = new Anchor
				{
					Width = new int?(10)
				},
				ValueChanged = delegate(float hue)
				{
					this._hsvPicker.SetHue(hue);
					bool flag = this.ResetTransparencyWhenChangingColor && this.Format == ColorPicker.ColorFormat.Rgba && (double)Math.Abs(this._opacitySelector.Opacity) < 0.0001;
					if (flag)
					{
						this._opacitySelector.Opacity = 1f;
						this._opacitySelector.Layout(null, true);
					}
				},
				MouseButtonReleased = delegate()
				{
					this.UpdateTextField(this.Value);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				}
			};
			this._opacitySelectorSpacer = new Group(desktop, this)
			{
				Anchor = new Anchor
				{
					Height = new int?(10)
				}
			};
			this._opacitySelector = new OpacitySelector(desktop, this)
			{
				Anchor = new Anchor
				{
					Height = new int?(10),
					Left = new int?(0),
					Right = new int?(20)
				},
				MouseButtonReleased = delegate()
				{
					this.UpdateTextField(this.Value);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				}
			};
			this._textField = new TextField(desktop, this)
			{
				Visible = false,
				ValueChanged = new Action(this.OnTextFieldValueChanged),
				Blurred = new Action(this.OnTextFieldBlurred),
				Anchor = new Anchor
				{
					Top = new int?(10)
				}
			};
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x0008B539 File Offset: 0x00089739
		protected override void OnMounted()
		{
			this.UpdateTextField(this.Value);
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0008B54C File Offset: 0x0008974C
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._opacitySelector.Visible = (this.Format == ColorPicker.ColorFormat.Rgba);
			this._opacitySelectorSpacer.Visible = (this.Format == ColorPicker.ColorFormat.Rgba);
			this._hsvPicker.Style = this.Style;
			this._hueSelector.Style = this.Style;
			this._opacitySelector.Style = this.Style;
			this._textField.Decoration = this.Style.TextFieldDecoration;
			this._textField.Style = this.Style.TextFieldInputStyle;
			this._textField.Padding = this.Style.TextFieldPadding;
			this._textField.Anchor.Height = new int?(this.Style.TextFieldHeight);
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0008B620 File Offset: 0x00089820
		public void SetColor(UInt32Color value, bool updateTextField = true)
		{
			ColorRgba colorRgba = ColorRgba.FromUInt32Color(value);
			ColorHsva colorHsva = ColorHsva.FromRgba(colorRgba.R, colorRgba.G, colorRgba.B, byte.MaxValue);
			this._hsvPicker.SetColor(colorHsva.H, colorHsva.S, colorHsva.V);
			this._hueSelector.Hue = colorHsva.H;
			this._opacitySelector.Opacity = (float)colorRgba.A / 255f;
			if (updateTextField)
			{
				this.UpdateTextField(value);
			}
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x0008B6A8 File Offset: 0x000898A8
		private void UpdateTextField(UInt32Color value)
		{
			switch (this._colorFormat)
			{
			case ColorPicker.ColorFormat.Rgb:
				this._textField.Value = value.ToHexString(false);
				break;
			case ColorPicker.ColorFormat.Rgba:
				this._textField.Value = value.ToHexString(true);
				break;
			case ColorPicker.ColorFormat.RgbShort:
				this._textField.Value = value.ToShortHexString();
				break;
			}
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x0008B714 File Offset: 0x00089914
		public ColorRgba GetColorRgba()
		{
			ColorHsva colorHsva = new ColorHsva(this._hsvPicker.Hue, this._hsvPicker.Saturation, this._hsvPicker.Value, 1f);
			byte r;
			byte g;
			byte b;
			byte b2;
			colorHsva.ToRgba(out r, out g, out b, out b2);
			return new ColorRgba(r, g, b, (byte)((int)(this._opacitySelector.Opacity * 255f)));
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0008B780 File Offset: 0x00089980
		private void OnTextFieldValueChanged()
		{
			string value = this._textField.Value.Trim();
			UInt32Color value2;
			bool flag = this.TryParseColor(value, out value2);
			if (flag)
			{
				this._wasTextFieldChanged = true;
				this.SetColor(value2, false);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0008B7D0 File Offset: 0x000899D0
		private bool TryParseColor(string value, out UInt32Color color)
		{
			bool flag = this.Format == ColorPicker.ColorFormat.RgbShort;
			if (flag)
			{
				bool flag2 = value.StartsWith("#") && value.Length == 4;
				if (flag2)
				{
					color = UInt32Color.FromShortHexString(value);
					return true;
				}
			}
			else
			{
				bool flag3 = this.Format == ColorPicker.ColorFormat.Rgb;
				if (flag3)
				{
					ColorUtils.ColorFormatType colorFormatType;
					bool flag4 = ColorUtils.TryParseColor(value, out color, out colorFormatType);
					if (flag4)
					{
						return true;
					}
				}
				else
				{
					ColorUtils.ColorFormatType colorFormatType;
					bool flag5 = ColorUtils.TryParseColorAlpha(value, out color, out colorFormatType);
					if (flag5)
					{
						return true;
					}
				}
			}
			color = UInt32Color.White;
			return false;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0008B868 File Offset: 0x00089A68
		private void OnTextFieldBlurred()
		{
			this.UpdateTextField(this.Value);
			bool wasTextFieldChanged = this._wasTextFieldChanged;
			if (wasTextFieldChanged)
			{
				this._wasTextFieldChanged = false;
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0008B8A8 File Offset: 0x00089AA8
		public override Element HitTest(Point position)
		{
			bool flag = !base.IsMounted || this._waitingForLayoutAfterMount || !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Element element = this._hsvPicker.HitTest(position);
				bool flag2 = element == null;
				if (flag2)
				{
					element = this._hueSelector.HitTest(position);
				}
				bool flag3 = element == null;
				if (flag3)
				{
					element = this._opacitySelector.HitTest(position);
				}
				bool flag4 = element == null;
				if (flag4)
				{
					element = this._textField.HitTest(position);
				}
				result = element;
			}
			return result;
		}

		// Token: 0x04001B4D RID: 6989
		private readonly HsvPicker _hsvPicker;

		// Token: 0x04001B4E RID: 6990
		private readonly Group _opacitySelectorSpacer;

		// Token: 0x04001B4F RID: 6991
		private readonly HueSelector _hueSelector;

		// Token: 0x04001B50 RID: 6992
		private readonly OpacitySelector _opacitySelector;

		// Token: 0x04001B51 RID: 6993
		private readonly TextField _textField;

		// Token: 0x04001B52 RID: 6994
		private ColorPicker.ColorFormat _colorFormat = ColorPicker.ColorFormat.Rgba;

		// Token: 0x04001B53 RID: 6995
		private bool _wasTextFieldChanged;

		// Token: 0x04001B54 RID: 6996
		[UIMarkupProperty]
		public ColorPickerStyle Style = new ColorPickerStyle();

		// Token: 0x04001B55 RID: 6997
		[UIMarkupProperty]
		public bool ResetTransparencyWhenChangingColor;

		// Token: 0x02000D26 RID: 3366
		public enum ColorFormat
		{
			// Token: 0x040040EB RID: 16619
			Rgb,
			// Token: 0x040040EC RID: 16620
			Rgba,
			// Token: 0x040040ED RID: 16621
			RgbShort
		}
	}
}
