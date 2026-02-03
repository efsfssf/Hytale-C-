using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000874 RID: 2164
	[UIMarkupElement]
	public class SliderNumberField : InputElement<int>
	{
		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x0009B47A File Offset: 0x0009967A
		// (set) Token: 0x06003D0F RID: 15631 RVA: 0x0009B487 File Offset: 0x00099687
		[UIMarkupProperty]
		public int Min
		{
			get
			{
				return this._slider.Min;
			}
			set
			{
				this._slider.Min = value;
				this._numberField.Format.MinValue = value;
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x0009B4AC File Offset: 0x000996AC
		// (set) Token: 0x06003D11 RID: 15633 RVA: 0x0009B4B9 File Offset: 0x000996B9
		[UIMarkupProperty]
		public int Max
		{
			get
			{
				return this._slider.Max;
			}
			set
			{
				this._slider.Max = value;
				this._numberField.Format.MaxValue = value;
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x0009B4DE File Offset: 0x000996DE
		// (set) Token: 0x06003D13 RID: 15635 RVA: 0x0009B4EB File Offset: 0x000996EB
		[UIMarkupProperty]
		public int Step
		{
			get
			{
				return this._slider.Step;
			}
			set
			{
				this._slider.Step = value;
				this._numberField.Format.Step = value;
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x0009B510 File Offset: 0x00099710
		// (set) Token: 0x06003D15 RID: 15637 RVA: 0x0009B51D File Offset: 0x0009971D
		public override int Value
		{
			get
			{
				return this._slider.Value;
			}
			set
			{
				this._slider.Value = value;
				this._numberField.Value = value;
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06003D16 RID: 15638 RVA: 0x0009B53F File Offset: 0x0009973F
		// (set) Token: 0x06003D17 RID: 15639 RVA: 0x0009B551 File Offset: 0x00099751
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

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06003D18 RID: 15640 RVA: 0x0009B564 File Offset: 0x00099764
		// (set) Token: 0x06003D19 RID: 15641 RVA: 0x0009B576 File Offset: 0x00099776
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

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06003D1A RID: 15642 RVA: 0x0009B589 File Offset: 0x00099789
		// (set) Token: 0x06003D1B RID: 15643 RVA: 0x0009B59B File Offset: 0x0009979B
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

		// Token: 0x1700107A RID: 4218
		// (set) Token: 0x06003D1C RID: 15644 RVA: 0x0009B5AE File Offset: 0x000997AE
		public Action SliderMouseButtonReleased
		{
			set
			{
				this._slider.MouseButtonReleased = value;
			}
		}

		// Token: 0x1700107B RID: 4219
		// (set) Token: 0x06003D1D RID: 15645 RVA: 0x0009B5BC File Offset: 0x000997BC
		public Action NumberFieldValidating
		{
			set
			{
				this._numberField.Validating = value;
			}
		}

		// Token: 0x1700107C RID: 4220
		// (set) Token: 0x06003D1E RID: 15646 RVA: 0x0009B5CA File Offset: 0x000997CA
		public Action NumberFieldDismissing
		{
			set
			{
				this._numberField.Dismissing = value;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (set) Token: 0x06003D1F RID: 15647 RVA: 0x0009B5D8 File Offset: 0x000997D8
		public Action NumberFieldBlurred
		{
			set
			{
				this._numberField.Blurred = value;
			}
		}

		// Token: 0x1700107E RID: 4222
		// (set) Token: 0x06003D20 RID: 15648 RVA: 0x0009B5E6 File Offset: 0x000997E6
		public Action NumberFieldFocused
		{
			set
			{
				this._numberField.Focused = value;
			}
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x0009B5F4 File Offset: 0x000997F4
		public SliderNumberField(Desktop desktop, Element parent) : base(desktop, parent)
		{
			Group parent2 = new Group(desktop, this)
			{
				LayoutMode = LayoutMode.Left
			};
			Group parent3 = new Group(desktop, parent2)
			{
				FlexWeight = 1
			};
			this._slider = new Slider(desktop, parent3);
			this._numberFieldContainer = new Group(desktop, parent2);
			this._numberField = new NumberField(desktop, this._numberFieldContainer);
			this._slider.ValueChanged = delegate()
			{
				this._numberField.Value = this._slider.Value;
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
					int value = (int)this._numberField.Value;
					this._slider.Value = value;
					this._slider.Layout(null, true);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				}
			};
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x0009B6DB File Offset: 0x000998DB
		protected override void ApplyStyles()
		{
			this._slider.Style = this.SliderStyle;
			this._numberFieldContainer.Anchor = this.NumberFieldContainerAnchor;
			this._numberField.Style = this.NumberFieldStyle;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x0009B711 File Offset: 0x00099911
		public override Element HitTest(Point position)
		{
			return this._slider.HitTest(position) ?? this._numberField.HitTest(position);
		}

		// Token: 0x04001C6D RID: 7277
		private Slider _slider;

		// Token: 0x04001C6E RID: 7278
		private NumberField _numberField;

		// Token: 0x04001C6F RID: 7279
		[UIMarkupProperty]
		public SliderStyle SliderStyle = SliderStyle.MakeDefault();

		// Token: 0x04001C70 RID: 7280
		[UIMarkupProperty]
		public InputFieldStyle NumberFieldStyle = new InputFieldStyle
		{
			RenderBold = true
		};

		// Token: 0x04001C71 RID: 7281
		private Group _numberFieldContainer;

		// Token: 0x04001C72 RID: 7282
		[UIMarkupProperty]
		public Anchor NumberFieldContainerAnchor = new Anchor
		{
			Left = new int?(15),
			Width = new int?(40),
			Height = new int?(15)
		};
	}
}
