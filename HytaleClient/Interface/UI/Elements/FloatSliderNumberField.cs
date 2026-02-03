using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000860 RID: 2144
	[UIMarkupElement]
	public class FloatSliderNumberField : InputElement<float>
	{
		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06003C00 RID: 15360 RVA: 0x00091FF2 File Offset: 0x000901F2
		// (set) Token: 0x06003C01 RID: 15361 RVA: 0x00091FFF File Offset: 0x000901FF
		[UIMarkupProperty]
		public float Min
		{
			get
			{
				return this._slider.Min;
			}
			set
			{
				this._slider.Min = value;
				this._numberField.Format.MinValue = (decimal)value;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06003C02 RID: 15362 RVA: 0x00092024 File Offset: 0x00090224
		// (set) Token: 0x06003C03 RID: 15363 RVA: 0x00092031 File Offset: 0x00090231
		[UIMarkupProperty]
		public float Max
		{
			get
			{
				return this._slider.Max;
			}
			set
			{
				this._slider.Max = value;
				this._numberField.Format.MaxValue = (decimal)value;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06003C04 RID: 15364 RVA: 0x00092056 File Offset: 0x00090256
		// (set) Token: 0x06003C05 RID: 15365 RVA: 0x00092063 File Offset: 0x00090263
		[UIMarkupProperty]
		public float Step
		{
			get
			{
				return this._slider.Step;
			}
			set
			{
				this._slider.Step = value;
				this._numberField.Format.Step = (decimal)value;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06003C06 RID: 15366 RVA: 0x00092088 File Offset: 0x00090288
		// (set) Token: 0x06003C07 RID: 15367 RVA: 0x00092095 File Offset: 0x00090295
		public override float Value
		{
			get
			{
				return this._slider.Value;
			}
			set
			{
				this._slider.Value = value;
				this._numberField.Value = (decimal)value;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06003C08 RID: 15368 RVA: 0x000920B7 File Offset: 0x000902B7
		// (set) Token: 0x06003C09 RID: 15369 RVA: 0x000920C9 File Offset: 0x000902C9
		[UIMarkupProperty]
		public int NumberFieldMaxDecimalPlaces
		{
			get
			{
				return this._numberField.Format.MaxDecimalPlaces;
			}
			set
			{
				this._numberField.Format.MaxDecimalPlaces = value;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06003C0A RID: 15370 RVA: 0x000920DC File Offset: 0x000902DC
		// (set) Token: 0x06003C0B RID: 15371 RVA: 0x000920EE File Offset: 0x000902EE
		[UIMarkupProperty]
		public decimal NumberFieldDefaultValue
		{
			get
			{
				return this._numberField.Format.DefaultValue;
			}
			set
			{
				this._numberField.Format.DefaultValue = value;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06003C0C RID: 15372 RVA: 0x00092101 File Offset: 0x00090301
		// (set) Token: 0x06003C0D RID: 15373 RVA: 0x00092113 File Offset: 0x00090313
		[UIMarkupProperty]
		public string NumberFieldSuffix
		{
			get
			{
				return this._numberField.Format.Suffix;
			}
			set
			{
				this._numberField.Format.Suffix = value;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (set) Token: 0x06003C0E RID: 15374 RVA: 0x00092126 File Offset: 0x00090326
		public Action SliderMouseButtonReleased
		{
			set
			{
				this._slider.MouseButtonReleased = value;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (set) Token: 0x06003C0F RID: 15375 RVA: 0x00092134 File Offset: 0x00090334
		public Action NumberFieldValidating
		{
			set
			{
				this._numberField.Validating = value;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (set) Token: 0x06003C10 RID: 15376 RVA: 0x00092142 File Offset: 0x00090342
		public Action NumberFieldDismissing
		{
			set
			{
				this._numberField.Dismissing = value;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (set) Token: 0x06003C11 RID: 15377 RVA: 0x00092150 File Offset: 0x00090350
		public Action NumberFieldBlurred
		{
			set
			{
				this._numberField.Blurred = value;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (set) Token: 0x06003C12 RID: 15378 RVA: 0x0009215E File Offset: 0x0009035E
		public Action NumberFieldFocused
		{
			set
			{
				this._numberField.Focused = value;
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0009216C File Offset: 0x0009036C
		public FloatSliderNumberField(Desktop desktop, Element parent) : base(desktop, parent)
		{
			Group parent2 = new Group(desktop, this)
			{
				LayoutMode = LayoutMode.Left
			};
			Group parent3 = new Group(desktop, parent2)
			{
				FlexWeight = 1
			};
			this._slider = new FloatSlider(desktop, parent3);
			this._numberFieldContainer = new Group(desktop, parent2);
			this._numberField = new NumberField(desktop, this._numberFieldContainer);
			this._slider.ValueChanged = delegate()
			{
				this._numberField.Value = (decimal)this._slider.Value;
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			};
			this._numberField.ValueChanged = delegate()
			{
				bool flag = !this._numberField.HasValidValue;
				if (!flag)
				{
					int num = (int)this._numberField.Value;
					this._slider.Value = (float)num;
					this._slider.Layout(null, true);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				}
			};
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00092253 File Offset: 0x00090453
		protected override void ApplyStyles()
		{
			this._slider.Style = this.SliderStyle;
			this._numberFieldContainer.Anchor = this.NumberFieldContainerAnchor;
			this._numberField.Style = this.NumberFieldStyle;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00092289 File Offset: 0x00090489
		public override Element HitTest(Point position)
		{
			return this._slider.HitTest(position) ?? this._numberField.HitTest(position);
		}

		// Token: 0x04001BBC RID: 7100
		private FloatSlider _slider;

		// Token: 0x04001BBD RID: 7101
		private NumberField _numberField;

		// Token: 0x04001BBE RID: 7102
		[UIMarkupProperty]
		public SliderStyle SliderStyle = SliderStyle.MakeDefault();

		// Token: 0x04001BBF RID: 7103
		[UIMarkupProperty]
		public InputFieldStyle NumberFieldStyle = new InputFieldStyle
		{
			RenderBold = true
		};

		// Token: 0x04001BC0 RID: 7104
		private Group _numberFieldContainer;

		// Token: 0x04001BC1 RID: 7105
		[UIMarkupProperty]
		public Anchor NumberFieldContainerAnchor = new Anchor
		{
			Left = new int?(15),
			Width = new int?(40),
			Height = new int?(15)
		};
	}
}
