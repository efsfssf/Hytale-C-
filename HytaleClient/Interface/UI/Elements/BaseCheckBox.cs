using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000851 RID: 2129
	public abstract class BaseCheckBox<CheckBoxStyleType, CheckBoxStyleStateType> : InputElement<bool> where CheckBoxStyleType : BaseCheckBoxStyle<CheckBoxStyleStateType>, new() where CheckBoxStyleStateType : CheckBoxStyleState, new()
	{
		// Token: 0x06003B18 RID: 15128 RVA: 0x0008A996 File Offset: 0x00088B96
		public BaseCheckBox(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0008A9A4 File Offset: 0x00088BA4
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			CheckBoxStyleStateType checkBoxStyleStateType = this.Value ? this.Style.Checked : this.Style.Unchecked;
			bool disabled = this.Disabled;
			if (disabled)
			{
				this._checkmarkPatch = this.Desktop.MakeTexturePatch(checkBoxStyleStateType.DisabledBackground ?? checkBoxStyleStateType.DefaultBackground);
			}
			else
			{
				int? capturedMouseButton = base.CapturedMouseButton;
				long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
				long num2 = (long)((ulong)1);
				bool flag = num.GetValueOrDefault() == num2 & num != null;
				if (flag)
				{
					Desktop desktop = this.Desktop;
					PatchStyle style;
					if ((style = checkBoxStyleStateType.PressedBackground) == null)
					{
						style = (checkBoxStyleStateType.HoveredBackground ?? checkBoxStyleStateType.DefaultBackground);
					}
					this._checkmarkPatch = desktop.MakeTexturePatch(style);
				}
				else
				{
					bool isHovered = base.IsHovered;
					if (isHovered)
					{
						this._checkmarkPatch = this.Desktop.MakeTexturePatch(checkBoxStyleStateType.HoveredBackground ?? checkBoxStyleStateType.DefaultBackground);
					}
					else
					{
						this._checkmarkPatch = this.Desktop.MakeTexturePatch(checkBoxStyleStateType.DefaultBackground);
					}
				}
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x0008AAFD File Offset: 0x00088CFD
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06003B1B RID: 15131 RVA: 0x0008AB11 File Offset: 0x00088D11
		public CheckBoxStyleState StyleState
		{
			get
			{
				return this.Value ? this.Style.Checked : this.Style.Unchecked;
			}
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0008AB44 File Offset: 0x00088D44
		protected override void OnMouseEnter()
		{
			bool flag = !this.Disabled;
			if (flag)
			{
				bool flag2 = this.StyleState.HoveredSound != null;
				if (flag2)
				{
					this.Desktop.Provider.PlaySound(this.StyleState.HoveredSound);
				}
			}
			base.Layout(null, true);
			SDL.SDL_SetCursor(this.Desktop.Cursors.Hand);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x0008ABB8 File Offset: 0x00088DB8
		protected override void OnMouseLeave()
		{
			base.Layout(null, true);
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0008ABF0 File Offset: 0x00088DF0
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool disabled = this.Disabled;
			if (!disabled)
			{
				bool flag = (long)evt.Button == 1L;
				if (flag)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0008AC2C File Offset: 0x00088E2C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool disabled = this.Disabled;
			if (!disabled)
			{
				bool flag = activate && (long)evt.Button == 1L;
				if (flag)
				{
					this.Value = !this.Value;
					base.Layout(null, true);
					Action valueChanged = this.ValueChanged;
					if (valueChanged != null)
					{
						valueChanged();
					}
				}
				bool flag2 = this.StyleState.ChangedSound != null;
				if (flag2)
				{
					this.Desktop.Provider.PlaySound(this.StyleState.ChangedSound);
				}
			}
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x0008ACC7 File Offset: 0x00088EC7
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			this.Desktop.Batcher2D.RequestDrawPatch(this._checkmarkPatch, this._rectangleAfterPadding, this.Desktop.Scale);
		}

		// Token: 0x04001B45 RID: 6981
		[UIMarkupProperty]
		public bool Disabled;

		// Token: 0x04001B46 RID: 6982
		[UIMarkupProperty]
		public CheckBoxStyleType Style;

		// Token: 0x04001B47 RID: 6983
		private TexturePatch _checkmarkPatch;
	}
}
