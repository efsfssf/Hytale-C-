using System;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBB RID: 3003
	internal class ColorEditor : ValueEditor
	{
		// Token: 0x06005DFD RID: 24061 RVA: 0x001DFBF4 File Offset: 0x001DDDF4
		public ColorEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005DFE RID: 24062 RVA: 0x001DFC18 File Offset: 0x001DDE18
		protected override void Build()
		{
			UInt32Color color = this.GetColor();
			TextField textField = new TextField(this.Desktop, this);
			SchemaNode parentSchema = this._parentSchema;
			textField.Visible = (parentSchema == null || parentSchema.Type != SchemaNode.NodeType.Timeline);
			textField.Value = (((string)base.Value) ?? "");
			textField.PlaceholderText = (string)this.Schema.DefaultValue;
			textField.Decoration = new InputFieldDecorationStyle
			{
				Focused = new InputFieldDecorationStyleState
				{
					OutlineSize = new int?(1),
					OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(244, 188, 81, 153))
				}
			};
			textField.PlaceholderStyle = new InputFieldStyle
			{
				TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100)
			};
			textField.Style = new InputFieldStyle
			{
				FontSize = 14f
			};
			textField.Padding = new Padding
			{
				Left = new int?(32)
			};
			textField.Anchor = new Anchor
			{
				Left = new int?(1)
			};
			textField.FlexWeight = 1;
			textField.Blurred = new Action(this.OnTextElementBlur);
			textField.Dismissing = new Action(this.OnTextElementDismissing);
			textField.Validating = delegate()
			{
				this._textInput.Blurred();
			};
			this._textInput = textField;
			this._colorPickerDropdownBox = new ColorPickerDropdownBox(this.Desktop, this)
			{
				Color = color,
				Format = this.Schema.ColorFormat,
				ValueChanged = new Action(this.OnColorPickerChange),
				Style = this.ConfigEditor.ColorPickerDropdownBoxStyle,
				DisplayTextField = true,
				Anchor = new Anchor
				{
					Width = new int?(24),
					Height = new int?(24),
					Left = new int?(4),
					Top = new int?(4)
				}
			};
		}

		// Token: 0x06005DFF RID: 24063 RVA: 0x001DFE28 File Offset: 0x001DE028
		private UInt32Color GetColor()
		{
			UInt32Color result = (this.Schema.ColorFormat == ColorPicker.ColorFormat.Rgba) ? UInt32Color.Transparent : UInt32Color.White;
			string text = (base.Value != null) ? ((string)base.Value).Trim() : "";
			bool flag = text == "" && this.Schema.DefaultValue != null;
			if (flag)
			{
				text = (string)this.Schema.DefaultValue;
			}
			bool flag2 = false;
			bool flag3 = text != "";
			if (flag3)
			{
				flag2 = this.TryParseColor(text, out result);
			}
			bool flag4 = !flag2 && this.Schema.DefaultValue != null;
			if (flag4)
			{
				this.TryParseColor((string)this.Schema.DefaultValue, out result);
			}
			return result;
		}

		// Token: 0x06005E00 RID: 24064 RVA: 0x001DFF04 File Offset: 0x001DE104
		private bool TryParseColor(string value, out UInt32Color color)
		{
			bool flag = this.Schema.ColorFormat == ColorPicker.ColorFormat.RgbShort;
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
				bool flag3 = this.Schema.ColorFormat == ColorPicker.ColorFormat.Rgb;
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

		// Token: 0x06005E01 RID: 24065 RVA: 0x001DFFA4 File Offset: 0x001DE1A4
		private void OnTextElementDismissing()
		{
			this.UpdateDisplayedValue();
		}

		// Token: 0x06005E02 RID: 24066 RVA: 0x001DFFB0 File Offset: 0x001DE1B0
		private void OnTextElementBlur()
		{
			string text = this._textInput.Value.Trim();
			bool flag = text == "";
			if (flag)
			{
				bool flag2 = base.Value != null;
				if (flag2)
				{
					this.ParentPropertyEditor.HandleRemoveProperty(false);
				}
			}
			else
			{
				UInt32Color color;
				bool flag3 = !this.TryParseColor(text, out color);
				if (flag3)
				{
					this._textInput.Value = (((string)base.Value) ?? "");
				}
				else
				{
					bool flag4 = (string)base.Value == this._textInput.Value;
					if (!flag4)
					{
						this._colorPickerDropdownBox.Color = color;
						base.HandleChangeValue(JToken.FromObject(this._textInput.Value), false, false, false);
						this.Validate();
					}
				}
			}
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x001E0084 File Offset: 0x001DE284
		private void OnColorPickerChange()
		{
			bool flag = this.Schema.ColorFormat == ColorPicker.ColorFormat.RgbShort;
			string text;
			if (flag)
			{
				UInt32Color color = this._colorPickerDropdownBox.Color;
				text = color.ToShortHexString();
			}
			else
			{
				bool flag2 = this.Schema.ColorFormat == ColorPicker.ColorFormat.Rgba;
				if (flag2)
				{
					UInt32Color color;
					ColorUtils.ColorFormatType formatType;
					bool flag3 = !ColorUtils.TryParseColorAlpha(this._textInput.Value.Trim(), out color, out formatType);
					if (flag3)
					{
						formatType = ColorUtils.ColorFormatType.HexAlpha;
					}
					text = ColorUtils.FormatColor(this._colorPickerDropdownBox.Color, formatType);
				}
				else
				{
					UInt32Color color;
					ColorUtils.ColorFormatType formatType2;
					bool flag4 = !ColorUtils.TryParseColor(this._textInput.Value.Trim(), out color, out formatType2);
					if (flag4)
					{
						formatType2 = ((this.Schema.ColorFormat == ColorPicker.ColorFormat.Rgba) ? ColorUtils.ColorFormatType.HexAlpha : ColorUtils.ColorFormatType.Hex);
					}
					text = ColorUtils.FormatColor(this._colorPickerDropdownBox.Color, formatType2);
				}
			}
			base.HandleChangeValue(JToken.FromObject(text), false, false, false);
			this._textInput.Value = text;
			this.Validate();
		}

		// Token: 0x06005E04 RID: 24068 RVA: 0x001E0180 File Offset: 0x001DE380
		protected internal override void UpdateDisplayedValue()
		{
			UInt32Color color = this.GetColor();
			this._colorPickerDropdownBox.Color = color;
			this._textInput.Value = (((string)base.Value) ?? "");
		}

		// Token: 0x06005E05 RID: 24069 RVA: 0x001E01C4 File Offset: 0x001DE3C4
		protected override bool IsValueEmptyOrDefault(JToken value)
		{
			bool flag = this.Schema.DefaultValue == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = (string)value == (string)this.Schema.DefaultValue;
				if (flag2)
				{
					result = true;
				}
				else
				{
					UInt32Color uint32Color;
					UInt32Color uint32Color2;
					bool flag3 = this.TryParseColor((string)value, out uint32Color) && this.TryParseColor((string)this.Schema.DefaultValue, out uint32Color2);
					result = (flag3 && uint32Color.Equals(uint32Color2));
				}
			}
			return result;
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x001E025A File Offset: 0x001DE45A
		public override void Focus()
		{
			this._colorPickerDropdownBox.Open();
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x001E0268 File Offset: 0x001DE468
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8;
		}

		// Token: 0x04003ABA RID: 15034
		private ColorPickerDropdownBox _colorPickerDropdownBox;

		// Token: 0x04003ABB RID: 15035
		private TextField _textInput;
	}
}
