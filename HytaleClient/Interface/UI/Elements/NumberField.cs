using System;
using System.Globalization;
using HytaleClient.Interface.UI.Markup;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000869 RID: 2153
	[UIMarkupElement]
	public class NumberField : InputField<decimal>
	{
		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x00098689 File Offset: 0x00096889
		// (set) Token: 0x06003CB5 RID: 15541 RVA: 0x00098694 File Offset: 0x00096894
		public override decimal Value
		{
			get
			{
				return this._value;
			}
			set
			{
				bool flag = value != this._value;
				if (flag)
				{
					this._value = value;
					this.HasValidValue = true;
					this.UpdateText();
				}
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x000986CA File Offset: 0x000968CA
		// (set) Token: 0x06003CB7 RID: 15543 RVA: 0x000986D2 File Offset: 0x000968D2
		public bool HasValidValue { get; private set; }

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000986DB File Offset: 0x000968DB
		public NumberField(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000986F2 File Offset: 0x000968F2
		protected override void OnMounted()
		{
			base.OnMounted();
			this.UpdateText();
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00098703 File Offset: 0x00096903
		protected override void LayoutSelf()
		{
			this.UpdateText();
			base.LayoutSelf();
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00098714 File Offset: 0x00096914
		private bool TryParseAsDecimal(out decimal value)
		{
			string text = this._text.ToLowerInvariant().Trim();
			bool flag = this.Format.Suffix != null;
			if (flag)
			{
				string text2 = this.Format.Suffix.ToLowerInvariant().Trim();
				bool flag2 = text.EndsWith(text2);
				if (flag2)
				{
					text = text.Substring(0, text.Length - text2.Length).Trim();
				}
			}
			return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00098798 File Offset: 0x00096998
		private void ClampValue(ref decimal value)
		{
			bool flag = value < this.Format.MinValue;
			if (flag)
			{
				value = this.Format.MinValue;
			}
			bool flag2 = value > this.Format.MaxValue;
			if (flag2)
			{
				value = this.Format.MaxValue;
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x000987FC File Offset: 0x000969FC
		private void SetValueFromDecimal(decimal value)
		{
			this.ClampValue(ref value);
			this.HasValidValue = true;
			this._value = value;
			this.UpdateText();
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x00098820 File Offset: 0x00096A20
		private void NudgeNumberValue(bool increment)
		{
			decimal num;
			bool flag = !this.TryParseAsDecimal(out num);
			if (flag)
			{
				bool flag2 = string.IsNullOrWhiteSpace(this._text);
				if (!flag2)
				{
					return;
				}
				num = this.Format.DefaultValue;
			}
			try
			{
				num += (increment ? this.Format.Step : (-this.Format.Step));
			}
			catch (OverflowException)
			{
				return;
			}
			this.SetValueFromDecimal(num);
			Action valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged();
			}
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x000988B8 File Offset: 0x00096AB8
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			if (keycode != SDL.SDL_Keycode.SDLK_DOWN)
			{
				if (keycode == SDL.SDL_Keycode.SDLK_UP)
				{
					this.NudgeNumberValue(true);
				}
			}
			else
			{
				this.NudgeNumberValue(false);
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x000988FC File Offset: 0x00096AFC
		protected override void OnTextChanged()
		{
			bool flag = string.IsNullOrWhiteSpace(this._text);
			if (flag)
			{
				this._value = this.Format.DefaultValue;
				this.HasValidValue = true;
				this.UpdateText();
			}
			else
			{
				decimal value;
				bool flag2 = !this.TryParseAsDecimal(out value);
				if (flag2)
				{
					bool flag3 = !this._isFocused;
					if (flag3)
					{
						this.HasValidValue = true;
						this.UpdateText();
					}
					else
					{
						this.HasValidValue = false;
					}
				}
				else
				{
					this.ClampValue(ref value);
					this.HasValidValue = true;
					this._value = value;
				}
			}
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00098990 File Offset: 0x00096B90
		protected internal override void OnBlur()
		{
			decimal value = this._value;
			this.ClampValue(ref this._value);
			this.HasValidValue = true;
			this.UpdateText();
			bool flag = value != this._value;
			if (flag)
			{
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
			base.OnBlur();
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x000989EC File Offset: 0x00096BEC
		private void UpdateText()
		{
			string text = this._text;
			string str = this.Format.Suffix ?? "";
			bool flag = this.Format.MaxDecimalPlaces > 0;
			if (flag)
			{
				this._text = ((this._value == this.Format.DefaultValue) ? "" : (this._value.ToString("0." + new string('#', this.Format.MaxDecimalPlaces), CultureInfo.InvariantCulture) + str));
				this._placeholderText = this.Format.DefaultValue.ToString("0." + new string('#', this.Format.MaxDecimalPlaces), CultureInfo.InvariantCulture) + str;
			}
			else
			{
				this._text = ((this._value == this.Format.DefaultValue) ? "" : (this._value.ToString("0", CultureInfo.InvariantCulture) + str));
				this._placeholderText = this.Format.DefaultValue.ToString("0", CultureInfo.InvariantCulture) + str;
			}
			bool flag2 = this._text != text;
			if (flag2)
			{
				base.CursorIndex = this._text.Length;
			}
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00098B50 File Offset: 0x00096D50
		protected internal override void Validate()
		{
			decimal value = this._value;
			this.ClampValue(ref this._value);
			this.HasValidValue = true;
			this.UpdateText();
			bool flag = value != this._value;
			if (flag)
			{
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
			base.Validate();
		}

		// Token: 0x04001C2A RID: 7210
		private decimal _value;

		// Token: 0x04001C2C RID: 7212
		[UIMarkupProperty]
		public NumberFieldFormat Format = new NumberFieldFormat();
	}
}
