using System;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000850 RID: 2128
	public abstract class BaseButton<ButtonStyleType, ButtonStyleStateType> : Element where ButtonStyleType : BaseButtonStyle<ButtonStyleStateType>, new() where ButtonStyleStateType : class, new()
	{
		// Token: 0x06003B0C RID: 15116 RVA: 0x0008A4AB File Offset: 0x000886AB
		public BaseButton(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0008A4C4 File Offset: 0x000886C4
		protected override void OnUnmounted()
		{
			bool isFocused = this._isFocused;
			if (isFocused)
			{
				this.Desktop.FocusElement(null, true);
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0008A4EC File Offset: 0x000886EC
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			this.ApplyStyles();
			return base.ComputeScaledMinSize(maxWidth, maxHeight);
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0008A510 File Offset: 0x00088710
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			bool flag = this.Disabled && this.Style.Disabled != null;
			if (flag)
			{
				this._styleState = this.Style.Disabled;
			}
			else
			{
				bool flag2;
				if (!this.Disabled)
				{
					int? capturedMouseButton = base.CapturedMouseButton;
					long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
					long num2 = (long)((ulong)1);
					if (num.GetValueOrDefault() == num2 & num != null)
					{
						flag2 = (this.Style.Pressed != null);
						goto IL_AF;
					}
				}
				flag2 = false;
				IL_AF:
				bool flag3 = flag2;
				if (flag3)
				{
					this._styleState = this.Style.Pressed;
				}
				else
				{
					bool flag4 = !this.Disabled && base.IsHovered && this.Style.Hovered != null;
					if (flag4)
					{
						this._styleState = this.Style.Hovered;
					}
					else
					{
						this._styleState = this.Style.Default;
					}
				}
			}
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0008A648 File Offset: 0x00088848
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

		// Token: 0x06003B11 RID: 15121 RVA: 0x0008A68C File Offset: 0x0008888C
		protected override void OnMouseEnter()
		{
			bool flag = !this.Disabled;
			if (flag)
			{
				base.Layout(null, true);
			}
			Action mouseEntered = this.MouseEntered;
			if (mouseEntered != null)
			{
				mouseEntered();
			}
			bool flag2 = !this.Disabled;
			if (flag2)
			{
				SDL.SDL_SetCursor(this.Desktop.Cursors.Hand);
				ButtonSounds sounds = this.Style.Sounds;
				bool flag3 = ((sounds != null) ? sounds.MouseHover : null) != null;
				if (flag3)
				{
					this.Desktop.Provider.PlaySound(this.Style.Sounds.MouseHover);
				}
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0008A738 File Offset: 0x00088938
		protected override void OnMouseLeave()
		{
			bool flag = !this.Disabled;
			if (flag)
			{
				base.Layout(null, true);
			}
			Action mouseExited = this.MouseExited;
			if (mouseExited != null)
			{
				mouseExited();
			}
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0008A78C File Offset: 0x0008898C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = !this.Disabled && (long)evt.Button == 1L;
			if (flag)
			{
				this.Desktop.FocusElement(this, true);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0008A7D8 File Offset: 0x000889D8
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			activate = (activate && !this.Disabled);
			bool flag = !this.Disabled;
			if (flag)
			{
				base.Layout(null, true);
			}
			bool flag2 = activate;
			if (flag2)
			{
				uint button = (uint)evt.Button;
				uint num = button;
				if (num != 1U)
				{
					if (num == 3U)
					{
						ButtonSounds sounds = this.Style.Sounds;
						bool flag3 = ((sounds != null) ? sounds.Context : null) != null;
						if (flag3)
						{
							IUIProvider provider = this.Desktop.Provider;
							ButtonSounds sounds2 = this.Style.Sounds;
							provider.PlaySound((sounds2 != null) ? sounds2.Context : null);
						}
						Action rightClicking = this.RightClicking;
						if (rightClicking != null)
						{
							rightClicking();
						}
					}
				}
				else
				{
					ButtonSounds sounds3 = this.Style.Sounds;
					bool flag4 = ((sounds3 != null) ? sounds3.Activate : null) != null;
					if (flag4)
					{
						IUIProvider provider2 = this.Desktop.Provider;
						ButtonSounds sounds4 = this.Style.Sounds;
						provider2.PlaySound((sounds4 != null) ? sounds4.Activate : null);
					}
					bool flag5 = this.DoubleClicking != null && evt.Clicks == 2 && this._isFocused;
					if (flag5)
					{
						this.DoubleClicking();
					}
					else
					{
						Action activating = this.Activating;
						if (activating != null)
						{
							activating();
						}
					}
				}
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x0008A938 File Offset: 0x00088B38
		protected internal override void OnBlur()
		{
			this._isFocused = false;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0008A942 File Offset: 0x00088B42
		protected internal override void OnFocus()
		{
			this._isFocused = true;
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x0008A94C File Offset: 0x00088B4C
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this._stateBackgroundPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._stateBackgroundPatch, this._anchoredRectangle, this.Desktop.Scale);
			}
		}

		// Token: 0x04001B3B RID: 6971
		[UIMarkupProperty]
		public bool Disabled;

		// Token: 0x04001B3C RID: 6972
		[UIMarkupProperty]
		public ButtonStyleType Style = Activator.CreateInstance<ButtonStyleType>();

		// Token: 0x04001B3D RID: 6973
		protected ButtonStyleStateType _styleState;

		// Token: 0x04001B3E RID: 6974
		protected TexturePatch _stateBackgroundPatch;

		// Token: 0x04001B3F RID: 6975
		protected bool _isFocused;

		// Token: 0x04001B40 RID: 6976
		public Action Activating;

		// Token: 0x04001B41 RID: 6977
		public Action DoubleClicking;

		// Token: 0x04001B42 RID: 6978
		public Action RightClicking;

		// Token: 0x04001B43 RID: 6979
		public Action MouseEntered;

		// Token: 0x04001B44 RID: 6980
		public Action MouseExited;
	}
}
