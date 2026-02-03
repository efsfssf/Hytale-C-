using System;
using System.Diagnostics;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000859 RID: 2137
	public class ColorPickerDropdownLayer : Element
	{
		// Token: 0x06003B61 RID: 15201 RVA: 0x0008C0C2 File Offset: 0x0008A2C2
		public ColorPickerDropdownLayer(ColorPickerDropdownBox dropdown) : base(dropdown.Desktop, null)
		{
			this._dropdown = dropdown;
			this.ColorPicker = new ColorPicker(this.Desktop, this);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x0008C0EC File Offset: 0x0008A2EC
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			ColorPickerDropdownBoxStyle style = this._dropdown.Style;
			this.ColorPicker.Style = style.ColorPickerStyle;
			this.ColorPicker.Background = style.PanelBackground;
			this.ColorPicker.Anchor.Width = new int?(style.PanelWidth);
			this.ColorPicker.Anchor.Height = new int?(style.PanelHeight);
			this.ColorPicker.Padding = style.PanelPadding;
			int num = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.X);
			int num2 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Top);
			int num3 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Width);
			int num4 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Height);
			int num5 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Height);
			int num6 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Width);
			this.ColorPicker.Anchor.Top = new int?(num2 + num4 + style.PanelOffset);
			int? num7 = this.ColorPicker.Anchor.Top + style.PanelHeight;
			int num8 = num5;
			bool flag = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag)
			{
				this.ColorPicker.Anchor.Top = new int?(num2 - style.PanelHeight - style.PanelOffset);
			}
			this.ColorPicker.Anchor.Left = new int?(num);
			num7 = this.ColorPicker.Anchor.Left + style.PanelWidth;
			num8 = num6;
			bool flag2 = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag2)
			{
				this.ColorPicker.Anchor.Left = new int?(num + num3 - style.PanelWidth);
			}
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0008C360 File Offset: 0x0008A560
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0008C3A4 File Offset: 0x0008A5A4
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button == 1L && !this.ColorPicker.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this._dropdown.CloseDropdown();
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0008C3F3 File Offset: 0x0008A5F3
		protected internal override void Dismiss()
		{
			this._dropdown.CloseDropdown();
		}

		// Token: 0x04001B5B RID: 7003
		public readonly ColorPicker ColorPicker;

		// Token: 0x04001B5C RID: 7004
		private readonly ColorPickerDropdownBox _dropdown;
	}
}
