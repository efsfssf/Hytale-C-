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
	// Token: 0x02000BC7 RID: 3015
	internal class NumberEditor : ValueEditor
	{
		// Token: 0x06005EB6 RID: 24246 RVA: 0x001E5ABC File Offset: 0x001E3CBC
		public NumberEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x001E5ADE File Offset: 0x001E3CDE
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005EB8 RID: 24248 RVA: 0x001E5AF0 File Offset: 0x001E3CF0
		protected override void Build()
		{
			NumberFieldFormat numberFieldFormat = new NumberFieldFormat
			{
				DefaultValue = ((this.Schema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this.Schema.DefaultValue) : 0m),
				MaxDecimalPlaces = this.Schema.MaxDecimalPlaces,
				Suffix = this.Schema.Suffix
			};
			bool flag = this.Schema.Min != null;
			if (flag)
			{
				numberFieldFormat.MinValue = JsonUtils.ConvertToDecimal(this.Schema.Min.Value);
			}
			bool flag2 = this.Schema.Max != null;
			if (flag2)
			{
				numberFieldFormat.MaxValue = JsonUtils.ConvertToDecimal(this.Schema.Max.Value);
			}
			bool flag3 = this.Schema.Step != null;
			if (flag3)
			{
				numberFieldFormat.Step = JsonUtils.ConvertToDecimal(this.Schema.Step.Value);
			}
			this._numberField = new NumberField(this.Desktop, this)
			{
				Value = this.GetValue(),
				Format = numberFieldFormat,
				Padding = new Padding
				{
					Left = new int?(5)
				},
				Anchor = new Anchor
				{
					Width = new int?(80),
					Left = new int?(0)
				},
				PlaceholderStyle = new InputFieldStyle
				{
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100),
					FontSize = 14f
				},
				Style = new InputFieldStyle
				{
					FontSize = 14f
				},
				Blurred = new Action(this.OnNumberFieldBlur),
				Validating = new Action(this.OnNumberFieldValidate),
				ValueChanged = new Action(this.OnNumberFieldChange),
				Decoration = new InputFieldDecorationStyle
				{
					Focused = new InputFieldDecorationStyleState
					{
						OutlineSize = new int?(1),
						OutlineColor = new UInt32Color?(UInt32Color.FromRGBA(244, 188, 81, 153))
					}
				}
			};
			SchemaNode parentSchema = this._parentSchema;
			bool flag4 = parentSchema != null && parentSchema.DisplayCompact;
			if (flag4)
			{
				this._numberField.FlexWeight = 1;
			}
			else
			{
				bool flag5 = this.Schema.Min != null && this.Schema.Max != null;
				if (flag5)
				{
					this._layoutMode = LayoutMode.Left;
					Group parent = new Group(this.Desktop, this)
					{
						FlexWeight = 1,
						Padding = new Padding
						{
							Horizontal = new int?(12)
						}
					};
					decimal d = (decimal)Math.Pow(10.0, (double)this.Schema.MaxDecimalPlaces);
					this._slider = new Slider(this.Desktop, parent)
					{
						Value = (int)(this.GetValue() * d),
						Style = this.ConfigEditor.SliderStyle,
						Min = (int)(JsonUtils.ConvertToDecimal(this.Schema.Min.Value) * d),
						Max = (int)(JsonUtils.ConvertToDecimal(this.Schema.Max.Value) * d),
						ValueChanged = new Action(this.OnSliderChanged),
						MouseButtonReleased = new Action(this.OnSliderReleased),
						Anchor = new Anchor
						{
							MaxWidth = new int?(250),
							Height = new int?(4),
							Left = new int?(0)
						}
					};
				}
			}
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x001E5EB0 File Offset: 0x001E40B0
		private decimal GetValue()
		{
			bool flag = base.Value == null;
			decimal result;
			if (flag)
			{
				result = ((this.Schema.DefaultValue != null) ? JsonUtils.ConvertToDecimal(this.Schema.DefaultValue) : 0m);
			}
			else
			{
				bool flag2 = base.Value.Type == 6 || base.Value.Type == 7;
				if (flag2)
				{
					result = JsonUtils.ConvertToDecimal(base.Value);
				}
				else
				{
					bool flag3 = this.Schema.MaxDecimalPlaces > 0 && base.Value.Type == 8;
					if (!flag3)
					{
						string str = "Invalid value type for ";
						JToken value = base.Value;
						throw new Exception(str + ((value != null) ? value.ToString() : null));
					}
					result = 0m;
				}
			}
			return result;
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x001E5F74 File Offset: 0x001E4174
		private void OnNumberFieldChange()
		{
			bool flag = this._slider != null;
			if (flag)
			{
				this._slider.Value = (int)(this._numberField.Value * (decimal)Math.Pow(10.0, (double)this.Schema.MaxDecimalPlaces));
				this._slider.Layout(null, true);
			}
			bool flag2 = this.Schema.MaxDecimalPlaces == 0;
			if (flag2)
			{
				base.HandleChangeValue((int)this._numberField.Value, true, false, false);
			}
			else
			{
				base.HandleChangeValue(this._numberField.Value, true, false, false);
			}
		}

		// Token: 0x06005EBB RID: 24251 RVA: 0x001E6038 File Offset: 0x001E4238
		private void OnSliderChanged()
		{
			decimal num = this._slider.Value / (decimal)Math.Pow(10.0, (double)this.Schema.MaxDecimalPlaces);
			this._numberField.Value = num;
			bool flag = this.Schema.MaxDecimalPlaces == 0;
			if (flag)
			{
				base.HandleChangeValue((int)num, true, false, false);
			}
			else
			{
				base.HandleChangeValue(num, true, false, false);
			}
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x001E60C5 File Offset: 0x001E42C5
		private void OnSliderReleased()
		{
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x001E60CF File Offset: 0x001E42CF
		private void OnNumberFieldValidate()
		{
			this.Validate();
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005EBE RID: 24254 RVA: 0x001E60E0 File Offset: 0x001E42E0
		private void OnNumberFieldBlur()
		{
			base.SubmitUpdateCommand();
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x001E60EC File Offset: 0x001E42EC
		protected override bool IsValueEmptyOrDefault(JToken value)
		{
			double num = (this.Schema.DefaultValue != null) ? ((double)this.Schema.DefaultValue) : 0.0;
			return Math.Abs(num - (double)value) < 1E-05;
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x001E614A File Offset: 0x001E434A
		public override void Focus()
		{
			this.Desktop.FocusElement(this._numberField, true);
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x001E6160 File Offset: 0x001E4360
		protected internal override void UpdateDisplayedValue()
		{
			this._numberField.Value = this.GetValue();
			bool flag = this._slider != null;
			if (flag)
			{
				this._slider.Value = (int)(this._numberField.Value * (decimal)Math.Pow(10.0, (double)this.Schema.MaxDecimalPlaces));
				this._slider.Layout(null, true);
			}
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x001E61E8 File Offset: 0x001E43E8
		protected override bool ValidateType(JToken value)
		{
			bool flag = value.Type == 6 || value.Type == 7;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.Schema.MaxDecimalPlaces > 0 && value.Type == 8;
				if (flag2)
				{
					string a = (string)value;
					bool flag3 = a == "Infinity" || a == "-Infinity" || a == "NaN";
					if (flag3)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x001E6274 File Offset: 0x001E4474
		protected override JToken SanitizeValue(JToken value)
		{
			bool flag = value.Type == 6 || value.Type == 7;
			if (flag)
			{
				double num = (double)value;
				bool flag2;
				if (this.Schema.Min != null)
				{
					double num2 = num;
					double? num3 = this.Schema.Min;
					flag2 = (num2 < num3.GetValueOrDefault() & num3 != null);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					value = this.Schema.Min.Value;
				}
				else
				{
					bool flag4;
					if (this.Schema.Max != null)
					{
						double num4 = num;
						double? num3 = this.Schema.Max;
						flag4 = (num4 > num3.GetValueOrDefault() & num3 != null);
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						value = this.Schema.Max.Value;
					}
				}
			}
			return value;
		}

		// Token: 0x04003B13 RID: 15123
		private NumberField _numberField;

		// Token: 0x04003B14 RID: 15124
		private Slider _slider;
	}
}
