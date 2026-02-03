using System;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000865 RID: 2149
	public abstract class InputField<T> : InputElement<T>
	{
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x00092F8F File Offset: 0x0009118F
		// (set) Token: 0x06003C47 RID: 15431 RVA: 0x00092F97 File Offset: 0x00091197
		public int CursorIndex
		{
			get
			{
				return this._cursorIndex;
			}
			set
			{
				this._cursorIndex = MathHelper.Clamp(value, 0, this._text.Length);
				this._relativeSelectionOffset = 0;
				this._cursorTimer = 0f;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06003C48 RID: 15432 RVA: 0x00092FC4 File Offset: 0x000911C4
		// (set) Token: 0x06003C49 RID: 15433 RVA: 0x00092FCC File Offset: 0x000911CC
		public int RelativeSelectionOffset
		{
			get
			{
				return this._relativeSelectionOffset;
			}
			set
			{
				this._relativeSelectionOffset = MathHelper.Clamp(value, -this._cursorIndex, this._text.Length - this._cursorIndex);
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06003C4A RID: 15434 RVA: 0x00092FF4 File Offset: 0x000911F4
		private string DisplayedText
		{
			get
			{
				bool flag = this.PasswordChar != null;
				string result;
				if (flag)
				{
					result = new string(this.PasswordChar.Value, this._text.Length);
				}
				else
				{
					result = (this.Style.RenderUppercase ? this._text.ToUpperInvariant() : this._text);
				}
				return result;
			}
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x00093053 File Offset: 0x00091253
		public InputField(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x0009308C File Offset: 0x0009128C
		protected override void OnMounted()
		{
			this._isHoveringClearButton = false;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			bool flag = this.AutoFocus && this.Desktop.FocusedElement == null;
			if (flag)
			{
				this.Desktop.FocusElement(this, true);
				bool flag2 = !string.IsNullOrEmpty(this._text) && this.AutoSelectAll;
				if (flag2)
				{
					this.SelectAll();
				}
			}
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00093108 File Offset: 0x00091308
		protected override void OnUnmounted()
		{
			bool isFocused = this._isFocused;
			if (isFocused)
			{
				this.Desktop.FocusElement(null, true);
			}
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00093147 File Offset: 0x00091347
		protected virtual void Animate(float deltaTime)
		{
			this._cursorTimer += deltaTime;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x00093158 File Offset: 0x00091358
		protected internal override void OnFocus()
		{
			this._isFocused = true;
			Action focused = this.Focused;
			if (focused != null)
			{
				focused();
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ApplyStyles();
			}
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x00093190 File Offset: 0x00091390
		protected internal override void OnBlur()
		{
			this._isFocused = false;
			Action blurred = this.Blurred;
			if (blurred != null)
			{
				blurred();
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ApplyStyles();
			}
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x000931C8 File Offset: 0x000913C8
		protected override void ApplyParentScroll(Point scaledParentScroll)
		{
			this._iconRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			this._clearButtonRectangle.Offset(-scaledParentScroll.X, -scaledParentScroll.Y);
			base.ApplyParentScroll(scaledParentScroll);
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x00093207 File Offset: 0x00091407
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x0009321C File Offset: 0x0009141C
		protected override void ApplyStyles()
		{
			bool flag = this.Decoration != null;
			if (flag)
			{
				bool isFocused = this._isFocused;
				if (isFocused)
				{
					InputFieldDecorationStyleState focused = this.Decoration.Focused;
					PatchStyle background;
					if ((background = ((focused != null) ? focused.Background : null)) == null)
					{
						InputFieldDecorationStyleState @default = this.Decoration.Default;
						background = ((@default != null) ? @default.Background : null);
					}
					this.Background = background;
					InputFieldDecorationStyleState focused2 = this.Decoration.Focused;
					UInt32Color? uint32Color = (focused2 != null) ? focused2.OutlineColor : null;
					UInt32Color outlineColor;
					if (uint32Color == null)
					{
						InputFieldDecorationStyleState default2 = this.Decoration.Default;
						outlineColor = (((default2 != null) ? default2.OutlineColor : null) ?? UInt32Color.White);
					}
					else
					{
						outlineColor = uint32Color.GetValueOrDefault();
					}
					this.OutlineColor = outlineColor;
					InputFieldDecorationStyleState focused3 = this.Decoration.Focused;
					int? num = (focused3 != null) ? focused3.OutlineSize : null;
					float valueOrDefault;
					if (num == null)
					{
						InputFieldDecorationStyleState default3 = this.Decoration.Default;
						valueOrDefault = (float)((default3 != null) ? default3.OutlineSize : null).GetValueOrDefault();
					}
					else
					{
						valueOrDefault = (float)num.GetValueOrDefault();
					}
					this.OutlineSize = valueOrDefault;
					InputFieldDecorationStyleState focused4 = this.Decoration.Focused;
					InputFieldIcon icon;
					if ((icon = ((focused4 != null) ? focused4.Icon : null)) == null)
					{
						InputFieldDecorationStyleState default4 = this.Decoration.Default;
						icon = ((default4 != null) ? default4.Icon : null);
					}
					this._icon = icon;
					InputFieldDecorationStyleState focused5 = this.Decoration.Focused;
					InputFieldButtonStyle clearButtonStyle;
					if ((clearButtonStyle = ((focused5 != null) ? focused5.ClearButtonStyle : null)) == null)
					{
						InputFieldDecorationStyleState default5 = this.Decoration.Default;
						clearButtonStyle = ((default5 != null) ? default5.ClearButtonStyle : null);
					}
					this._clearButtonStyle = clearButtonStyle;
				}
				else
				{
					InputFieldDecorationStyleState default6 = this.Decoration.Default;
					this.Background = ((default6 != null) ? default6.Background : null);
					InputFieldDecorationStyleState default7 = this.Decoration.Default;
					this.OutlineColor = (((default7 != null) ? default7.OutlineColor : null) ?? UInt32Color.White);
					InputFieldDecorationStyleState default8 = this.Decoration.Default;
					this.OutlineSize = (float)((default8 != null) ? default8.OutlineSize : null).GetValueOrDefault();
					InputFieldDecorationStyleState default9 = this.Decoration.Default;
					this._icon = ((default9 != null) ? default9.Icon : null);
					InputFieldDecorationStyleState default10 = this.Decoration.Default;
					this._clearButtonStyle = ((default10 != null) ? default10.ClearButtonStyle : null);
				}
			}
			base.ApplyStyles();
			this._font = this.Desktop.Provider.GetFontFamily(this.Style.FontName.Value).RegularFont;
			this._placeholderFont = this.Desktop.Provider.GetFontFamily(this.PlaceholderStyle.FontName.Value).RegularFont;
			this._iconPatch = ((this._icon != null) ? this.Desktop.MakeTexturePatch(this._icon.Texture) : null);
			bool isPressingClearButton = this._isPressingClearButton;
			if (isPressingClearButton)
			{
				TexturePatch clearButtonPatch;
				if (this._clearButtonStyle == null)
				{
					clearButtonPatch = null;
				}
				else
				{
					Desktop desktop = this.Desktop;
					PatchStyle style;
					if ((style = this._clearButtonStyle.PressedTexture) == null)
					{
						style = (this._clearButtonStyle.HoveredTexture ?? this._clearButtonStyle.Texture);
					}
					clearButtonPatch = desktop.MakeTexturePatch(style);
				}
				this._clearButtonPatch = clearButtonPatch;
			}
			else
			{
				bool isHoveringClearButton = this._isHoveringClearButton;
				if (isHoveringClearButton)
				{
					this._clearButtonPatch = ((this._clearButtonStyle != null) ? this.Desktop.MakeTexturePatch(this._clearButtonStyle.HoveredTexture ?? this._clearButtonStyle.Texture) : null);
				}
				else
				{
					this._clearButtonPatch = ((this._clearButtonStyle != null) ? this.Desktop.MakeTexturePatch(this._clearButtonStyle.Texture) : null);
				}
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000935DC File Offset: 0x000917DC
		protected override void LayoutSelf()
		{
			bool flag = this._iconPatch != null;
			if (flag)
			{
				int num = this.Desktop.ScaleRound((float)this._icon.Height);
				int num2 = this.Desktop.ScaleRound((float)this._icon.Width);
				int x = (this._icon.Side == InputFieldIcon.InputFieldIconSide.Left) ? (this._anchoredRectangle.Left + this.Desktop.ScaleRound((float)this._icon.Offset)) : (this._anchoredRectangle.Right - num2 - this.Desktop.ScaleRound((float)this._icon.Offset));
				int y = this._anchoredRectangle.Top + (int)((float)this._anchoredRectangle.Height / 2f - (float)num / 2f);
				this._iconRectangle = new Rectangle(x, y, num2, num);
			}
			bool flag2 = this._clearButtonPatch != null;
			if (flag2)
			{
				int num3 = this.Desktop.ScaleRound((float)this._clearButtonStyle.Height);
				int num4 = this.Desktop.ScaleRound((float)this._clearButtonStyle.Width);
				int x2 = (this._clearButtonStyle.Side == InputFieldButtonStyle.InputFieldButtonSide.Left) ? (this._anchoredRectangle.Left + this.Desktop.ScaleRound((float)this._clearButtonStyle.Offset)) : (this._anchoredRectangle.Right - num4 - this.Desktop.ScaleRound((float)this._clearButtonStyle.Offset));
				int y2 = this._anchoredRectangle.Top + (int)((float)this._anchoredRectangle.Height / 2f - (float)num3 / 2f);
				this._clearButtonRectangle = new Rectangle(x2, y2, num4, num3);
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0009379C File Offset: 0x0009199C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			this._isPressingClearButton = false;
			bool flag = activate && this._clearButtonStyle != null && this._clearButtonRectangle.Contains(this.Desktop.MousePosition) && this._text != "";
			if (flag)
			{
				this._text = "";
				this.CursorIndex = 0;
				this.OnTextChanged();
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ApplyStyles();
			}
			bool flag2 = activate && (long)evt.Button == 3L;
			if (flag2)
			{
				Action rightClicking = this.RightClicking;
				if (rightClicking != null)
				{
					rightClicking();
				}
			}
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x00093854 File Offset: 0x00091A54
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = this._clearButtonStyle != null && this._clearButtonRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this._isPressingClearButton = true;
				this.ApplyStyles();
			}
			else
			{
				this._cursorTimer = 0f;
				this._mouseClickCount = evt.Clicks;
				switch (evt.Clicks)
				{
				case 1:
					this.Desktop.FocusElement(this, true);
					this.CursorIndex = this.GetCursorIndexAtPosition(this.Desktop.MousePosition.X);
					break;
				case 2:
				{
					bool isFocused = this._isFocused;
					if (isFocused)
					{
						this.SelectWordAt(this.GetCursorIndexAtPosition(this.Desktop.MousePosition.X));
					}
					break;
				}
				case 3:
				{
					bool isFocused2 = this._isFocused;
					if (isFocused2)
					{
						this.SelectAll();
					}
					break;
				}
				}
			}
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0009393F File Offset: 0x00091B3F
		protected override void OnMouseEnter()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.IBeam);
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x00093957 File Offset: 0x00091B57
		protected override void OnMouseLeave()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			this._isHoveringClearButton = false;
			this.ApplyStyles();
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x00093980 File Offset: 0x00091B80
		protected override void OnMouseMove()
		{
			bool flag;
			if (this._mouseClickCount == 1)
			{
				int? capturedMouseButton = base.CapturedMouseButton;
				long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
				long num2 = (long)((ulong)1);
				flag = (num.GetValueOrDefault() == num2 & num != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				int num3 = this.CursorIndex + this.RelativeSelectionOffset;
				this.CursorIndex = this.GetCursorIndexAtPosition(this.Desktop.MousePosition.X);
				this.RelativeSelectionOffset = num3 - this.CursorIndex;
			}
			bool flag3 = this._clearButtonStyle != null && this._clearButtonRectangle.Contains(this.Desktop.MousePosition);
			if (flag3)
			{
				bool flag4 = !this._isHoveringClearButton;
				if (flag4)
				{
					this._isHoveringClearButton = true;
					SDL.SDL_SetCursor(this.Desktop.Cursors.Hand);
					this.ApplyStyles();
				}
			}
			else
			{
				bool isHoveringClearButton = this._isHoveringClearButton;
				if (isHoveringClearButton)
				{
					this._isHoveringClearButton = false;
					SDL.SDL_SetCursor(this.Desktop.Cursors.IBeam);
					this.ApplyStyles();
				}
			}
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x00093AB0 File Offset: 0x00091CB0
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			InputField<T>.<>c__DisplayClass55_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			bool isShortcutKeyDown = this.Desktop.IsShortcutKeyDown;
			CS$<>8__locals1.isShiftDown = this.Desktop.IsShiftKeyDown;
			bool isWordSkipDown = this.Desktop.IsWordSkipDown;
			bool isLineSkipDown = this.Desktop.IsLineSkipDown;
			if (keycode <= SDL.SDL_Keycode.SDLK_c)
			{
				if (keycode != SDL.SDL_Keycode.SDLK_BACKSPACE)
				{
					if (keycode == SDL.SDL_Keycode.SDLK_a)
					{
						bool flag = isShortcutKeyDown;
						if (flag)
						{
							this.SelectAll();
						}
						goto IL_542;
					}
					if (keycode == SDL.SDL_Keycode.SDLK_c)
					{
						bool flag2 = isShortcutKeyDown && this.PasswordChar == null;
						if (flag2)
						{
							this.CopySelectionToClipboard();
						}
						goto IL_542;
					}
				}
				else
				{
					bool flag3 = this.IsReadOnly || this._text.Length == 0;
					if (flag3)
					{
						goto IL_542;
					}
					bool flag4 = this.RelativeSelectionOffset != 0;
					if (flag4)
					{
						bool flag5 = this.DeleteSelectedText();
						if (flag5)
						{
							this.OnTextChanged();
							Action valueChanged = this.ValueChanged;
							if (valueChanged != null)
							{
								valueChanged();
							}
						}
					}
					else
					{
						bool flag6 = this.CursorIndex > 0;
						if (flag6)
						{
							int num = this.CursorIndex;
							bool flag7 = isWordSkipDown;
							if (flag7)
							{
								num = this._text.LastIndexOf(' ', Math.Max(num - 2, 0)) + 1;
							}
							else
							{
								num--;
							}
							this._text = this._text.Remove(num, this.CursorIndex - num);
							this.CursorIndex = num;
							this.OnTextChanged();
							Action valueChanged2 = this.ValueChanged;
							if (valueChanged2 != null)
							{
								valueChanged2();
							}
						}
					}
					goto IL_542;
				}
			}
			else if (keycode <= SDL.SDL_Keycode.SDLK_x)
			{
				if (keycode == SDL.SDL_Keycode.SDLK_v)
				{
					bool flag8 = isShortcutKeyDown;
					if (flag8)
					{
						this.PasteFromClipboard();
					}
					goto IL_542;
				}
				if (keycode == SDL.SDL_Keycode.SDLK_x)
				{
					bool flag9 = !isShortcutKeyDown || this.PasswordChar != null;
					if (flag9)
					{
						goto IL_542;
					}
					this.CopySelectionToClipboard();
					bool flag10 = this.DeleteSelectedText();
					if (flag10)
					{
						this.OnTextChanged();
						Action valueChanged3 = this.ValueChanged;
						if (valueChanged3 != null)
						{
							valueChanged3();
						}
					}
					goto IL_542;
				}
			}
			else if (keycode != SDL.SDL_Keycode.SDLK_DELETE)
			{
				switch (keycode)
				{
				case SDL.SDL_Keycode.SDLK_HOME:
					this.<OnKeyDown>g__SkipToLineStart|55_0(ref CS$<>8__locals1);
					goto IL_542;
				case SDL.SDL_Keycode.SDLK_END:
					this.<OnKeyDown>g__SkipToLineEnd|55_1(ref CS$<>8__locals1);
					goto IL_542;
				case SDL.SDL_Keycode.SDLK_RIGHT:
				{
					bool flag11 = isLineSkipDown;
					if (flag11)
					{
						this.<OnKeyDown>g__SkipToLineEnd|55_1(ref CS$<>8__locals1);
					}
					else
					{
						bool flag12 = !CS$<>8__locals1.isShiftDown && this._relativeSelectionOffset != 0;
						if (flag12)
						{
							this.CursorIndex = Math.Max(this._cursorIndex, this._cursorIndex + this._relativeSelectionOffset);
						}
						else
						{
							int num2 = this._cursorIndex;
							bool flag13 = isWordSkipDown;
							if (flag13)
							{
								num2 = this._text.IndexOf(' ', num2, this._text.Length - num2) + 1;
								bool flag14 = num2 == 0;
								if (flag14)
								{
									num2 = this._text.Length;
								}
							}
							else
							{
								num2++;
							}
							bool isShiftDown = CS$<>8__locals1.isShiftDown;
							if (isShiftDown)
							{
								int num3 = this.CursorIndex + this.RelativeSelectionOffset;
								this.CursorIndex = num2;
								this.RelativeSelectionOffset = num3 - this.CursorIndex;
							}
							else
							{
								this.CursorIndex = num2;
							}
						}
					}
					goto IL_542;
				}
				case SDL.SDL_Keycode.SDLK_LEFT:
				{
					bool flag15 = isLineSkipDown;
					if (flag15)
					{
						this.<OnKeyDown>g__SkipToLineStart|55_0(ref CS$<>8__locals1);
					}
					else
					{
						bool flag16 = !CS$<>8__locals1.isShiftDown && this._relativeSelectionOffset != 0;
						if (flag16)
						{
							this.CursorIndex = Math.Min(this._cursorIndex, this._cursorIndex + this._relativeSelectionOffset);
						}
						else
						{
							int num4 = this._cursorIndex;
							bool flag17 = isWordSkipDown;
							if (flag17)
							{
								bool flag18 = num4 > 0;
								if (flag18)
								{
									num4 = this._text.LastIndexOf(' ', Math.Max(num4 - 2, 0)) + 1;
								}
							}
							else
							{
								num4--;
							}
							bool isShiftDown2 = CS$<>8__locals1.isShiftDown;
							if (isShiftDown2)
							{
								int num5 = this.CursorIndex + this.RelativeSelectionOffset;
								this.CursorIndex = num4;
								this.RelativeSelectionOffset = num5 - this.CursorIndex;
							}
							else
							{
								this.CursorIndex = num4;
							}
						}
					}
					goto IL_542;
				}
				}
			}
			else
			{
				bool flag19 = this.IsReadOnly || this._text.Length == 0;
				if (flag19)
				{
					goto IL_542;
				}
				bool flag20 = this.RelativeSelectionOffset != 0;
				if (flag20)
				{
					bool flag21 = this.DeleteSelectedText();
					if (flag21)
					{
						Action valueChanged4 = this.ValueChanged;
						if (valueChanged4 != null)
						{
							valueChanged4();
						}
					}
				}
				else
				{
					bool flag22 = this.CursorIndex < this._text.Length;
					if (flag22)
					{
						int num6 = this.CursorIndex;
						bool flag23 = isWordSkipDown;
						if (flag23)
						{
							num6 = this._text.IndexOf(' ', num6, this._text.Length - num6) + 1;
							bool flag24 = num6 == 0;
							if (flag24)
							{
								num6 = this._text.Length;
							}
						}
						else
						{
							num6++;
						}
						this._text = this._text.Remove(this.CursorIndex, num6 - this.CursorIndex);
						this.OnTextChanged();
						Action valueChanged5 = this.ValueChanged;
						if (valueChanged5 != null)
						{
							valueChanged5();
						}
					}
				}
				goto IL_542;
			}
			base.OnKeyDown(keycode, repeat);
			IL_542:
			Action<SDL.SDL_Keycode> keyDown = this.KeyDown;
			if (keyDown != null)
			{
				keyDown(keycode);
			}
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x00094012 File Offset: 0x00092212
		public void SelectAll()
		{
			this.CursorIndex = this._text.Length;
			this.RelativeSelectionOffset = -this._text.Length;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0009403C File Offset: 0x0009223C
		private void SelectWordAt(int cursorIndex)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			InputField<T>.CharType charType = InputField<T>.CharType.Other;
			for (int i = 0; i < this._text.Length; i++)
			{
				char c = this._text[i];
				bool flag2 = char.IsWhiteSpace(c);
				InputField<T>.CharType charType2;
				if (flag2)
				{
					charType2 = InputField<T>.CharType.Whitespace;
				}
				else
				{
					bool flag3 = char.IsLetter(c) || char.IsNumber(c);
					if (flag3)
					{
						charType2 = InputField<T>.CharType.AlphaNumeric;
					}
					else
					{
						charType2 = InputField<T>.CharType.Other;
					}
				}
				bool flag4 = charType2 != charType;
				if (flag4)
				{
					bool flag5 = flag;
					if (flag5)
					{
						break;
					}
					charType = charType2;
					num = i;
				}
				num2 = i;
				bool flag6 = i == cursorIndex;
				if (flag6)
				{
					flag = true;
					bool flag7 = charType2 == InputField<T>.CharType.Other;
					if (flag7)
					{
						num = i;
						break;
					}
				}
			}
			this.CursorIndex = num2 + 1;
			this.RelativeSelectionOffset = num - num2 - 1;
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x00094114 File Offset: 0x00092314
		public bool DeleteSelectedText()
		{
			bool flag = this.RelativeSelectionOffset > 0;
			bool result;
			if (flag)
			{
				this._text = this._text.Remove(this.CursorIndex, this.RelativeSelectionOffset);
				this.RelativeSelectionOffset = 0;
				result = true;
			}
			else
			{
				bool flag2 = this.RelativeSelectionOffset < 0;
				if (flag2)
				{
					int num = this.CursorIndex + this.RelativeSelectionOffset;
					this._text = this._text.Remove(num, -this.RelativeSelectionOffset);
					this.CursorIndex = num;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x000941A0 File Offset: 0x000923A0
		public void CopySelectionToClipboard()
		{
			bool flag = this.RelativeSelectionOffset != 0;
			if (flag)
			{
				bool flag2 = this.RelativeSelectionOffset > 0;
				string text;
				if (flag2)
				{
					text = this._text.Substring(this.CursorIndex, this.RelativeSelectionOffset);
				}
				else
				{
					text = this._text.Substring(this.CursorIndex + this.RelativeSelectionOffset, -this.RelativeSelectionOffset);
				}
				SDL.SDL_SetClipboardText(text);
			}
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0009420B File Offset: 0x0009240B
		private bool TextOverflowsMaxLength(string text)
		{
			return this.MaxLength > 0 && this._text.Length + text.Length > this.MaxLength && this.RelativeSelectionOffset == 0;
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0009423C File Offset: 0x0009243C
		public void PasteFromClipboard()
		{
			string text = SDL.SDL_GetClipboardText();
			bool flag = text != null;
			if (flag)
			{
				text = text.Replace("\r\n", " ").Replace("\n", " ");
				this.DeleteSelectedText();
				bool flag2 = this.TextOverflowsMaxLength(text);
				if (flag2)
				{
					text = text.Substring(0, this.MaxLength - this._text.Length);
				}
				this._text = this._text.Insert(this.CursorIndex, text);
				this.CursorIndex += text.Length;
				this.OnTextChanged();
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x000942F0 File Offset: 0x000924F0
		public void InsertAtCursor(string text)
		{
			bool isReadOnly = this.IsReadOnly;
			if (!isReadOnly)
			{
				this.DeleteSelectedText();
				bool flag = this.TextOverflowsMaxLength(text);
				if (flag)
				{
					text = text.Substring(0, this.MaxLength - this._text.Length);
				}
				this._text = this._text.Insert(this.CursorIndex, text);
				this.CursorIndex += text.Length;
				this.OnTextChanged();
			}
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x0009436C File Offset: 0x0009256C
		protected internal override void OnTextInput(string text)
		{
			bool flag = this.IsReadOnly || this.TextOverflowsMaxLength(text);
			if (!flag)
			{
				this.DeleteSelectedText();
				this._text = this._text.Insert(this.CursorIndex, text);
				int cursorIndex = this.CursorIndex;
				this.CursorIndex = cursorIndex + 1;
				this.OnTextChanged();
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x000943DC File Offset: 0x000925DC
		private int GetCursorIndexAtPosition(int x)
		{
			float num = this.Style.FontSize * this.Desktop.Scale;
			float num2 = num / (float)this._font.BaseSize;
			float num3 = (float)(x - this._rectangleAfterPadding.X + this._scrollOffset) / num2;
			float num4 = 0f;
			int num5 = 0;
			foreach (ushort key in this.DisplayedText)
			{
				float num6;
				bool flag = !this._font.GlyphAdvances.TryGetValue(key, out num6);
				if (!flag)
				{
					bool flag2 = num3 <= num4 + num6 / 2f;
					if (flag2)
					{
						break;
					}
					num4 += num6;
					num5++;
				}
			}
			return num5;
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x000944A8 File Offset: 0x000926A8
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this._iconPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._iconPatch, this._iconRectangle, this.Desktop.Scale);
			}
			bool flag2 = this._clearButtonPatch != null && this._text != "";
			if (flag2)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._clearButtonPatch, this._clearButtonRectangle, this.Desktop.Scale);
			}
			this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
			float num = this.Style.FontSize * this.Desktop.Scale;
			float num2 = num / (float)this._font.BaseSize;
			string displayedText = this.DisplayedText;
			int num3 = (int)(this._font.CalculateTextWidth(displayedText.Substring(0, this.CursorIndex)) * num2);
			int width = this._rectangleAfterPadding.Width;
			float num4 = this._font.CalculateTextWidth(this.DisplayedText) * num2 + 1f;
			int num5 = (int)Math.Max(0f, num4 - (float)width);
			this._scrollOffset = MathHelper.Clamp(this._scrollOffset, 0, num5);
			int num6 = this._rectangleAfterPadding.Width / 3;
			bool flag3 = this._rectangleAfterPadding.Width != 0 && num3 > this._scrollOffset + width - 1;
			if (flag3)
			{
				this._scrollOffset = Math.Min(num5, num3 - width + 1 + num6);
			}
			else
			{
				bool flag4 = num3 < this._scrollOffset;
				if (flag4)
				{
					this._scrollOffset = Math.Max(0, num3 - num6);
				}
			}
			int num7 = this._rectangleAfterPadding.X - this._scrollOffset;
			int num8 = (int)((float)this._font.Height * num2);
			int num9 = this._rectangleAfterPadding.Center.Y - (int)((float)num8 / 2f);
			bool flag5 = this._text.Length == 0 && this._placeholderText != null;
			if (flag5)
			{
				float num10 = this.PlaceholderStyle.FontSize * this.Desktop.Scale;
				float num11 = num10 / (float)this._placeholderFont.BaseSize;
				Vector3 position = new Vector3((float)this._rectangleAfterPadding.X, (float)(this._rectangleAfterPadding.Center.Y - (int)((float)this._placeholderFont.Height * num11 / 2f)), 0f);
				this.Desktop.Batcher2D.RequestDrawText(this._placeholderFont, num10, this.PlaceholderStyle.RenderUppercase ? this._placeholderText.ToUpperInvariant() : this._placeholderText, position, this.PlaceholderStyle.TextColor, this.PlaceholderStyle.RenderBold, this.PlaceholderStyle.RenderItalics, 0f);
			}
			else
			{
				this.Desktop.Batcher2D.RequestDrawText(this._font, num, displayedText, new Vector3((float)num7, (float)num9, 0f), this.Style.TextColor, this.Style.RenderBold, this.Style.RenderItalics, 0f);
			}
			bool flag6 = this.RelativeSelectionOffset != 0;
			if (flag6)
			{
				int val = (int)(this._font.CalculateTextWidth(displayedText.Substring(0, this.CursorIndex + this.RelativeSelectionOffset)) * num2);
				int num12 = Math.Min(num3, val);
				int num13 = Math.Max(num3, val);
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, new Rectangle(num7 + num12, num9, num13 - num12, num8), UInt32Color.FromRGBA(this._isFocused ? 4294967136U : 4294967072U));
			}
			bool flag7 = this._isFocused && this.Desktop.IsFocused && this._cursorTimer % 1f < 0.5f;
			if (flag7)
			{
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, new Rectangle(num7 + num3, num9, 1, num8), this.Style.TextColor);
			}
			this.Desktop.Batcher2D.PopScissor();
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x00094926 File Offset: 0x00092B26
		protected virtual void OnTextChanged()
		{
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0009492C File Offset: 0x00092B2C
		protected internal override void Validate()
		{
			bool flag = this._isFocused && this.Validating != null;
			if (flag)
			{
				this.Validating();
			}
			else
			{
				base.Validate();
			}
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x00094968 File Offset: 0x00092B68
		protected internal override void Dismiss()
		{
			bool flag = this._isFocused && this.Dismissing != null;
			if (flag)
			{
				this.Dismissing();
			}
			else
			{
				base.Dismiss();
			}
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x000949A4 File Offset: 0x00092BA4
		[CompilerGenerated]
		private void <OnKeyDown>g__SkipToLineStart|55_0(ref InputField<T>.<>c__DisplayClass55_0 A_1)
		{
			bool isShiftDown = A_1.isShiftDown;
			if (isShiftDown)
			{
				this.RelativeSelectionOffset = -this.CursorIndex;
			}
			else
			{
				this.CursorIndex = 0;
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x000949D4 File Offset: 0x00092BD4
		[CompilerGenerated]
		private void <OnKeyDown>g__SkipToLineEnd|55_1(ref InputField<T>.<>c__DisplayClass55_0 A_1)
		{
			bool isShiftDown = A_1.isShiftDown;
			if (isShiftDown)
			{
				int cursorIndex = this.CursorIndex;
				this.CursorIndex = this._text.Length;
				this.RelativeSelectionOffset = this._text.Length - cursorIndex;
			}
			else
			{
				this.CursorIndex = this._text.Length;
			}
		}

		// Token: 0x04001BDD RID: 7133
		[UIMarkupProperty]
		public InputFieldStyle Style = new InputFieldStyle();

		// Token: 0x04001BDE RID: 7134
		[UIMarkupProperty]
		public InputFieldStyle PlaceholderStyle = new InputFieldStyle();

		// Token: 0x04001BDF RID: 7135
		[UIMarkupProperty]
		public InputFieldDecorationStyle Decoration;

		// Token: 0x04001BE0 RID: 7136
		[UIMarkupProperty]
		public bool AutoFocus;

		// Token: 0x04001BE1 RID: 7137
		[UIMarkupProperty]
		public bool AutoSelectAll;

		// Token: 0x04001BE2 RID: 7138
		protected string _placeholderText;

		// Token: 0x04001BE3 RID: 7139
		private InputFieldIcon _icon;

		// Token: 0x04001BE4 RID: 7140
		private TexturePatch _iconPatch;

		// Token: 0x04001BE5 RID: 7141
		private Rectangle _iconRectangle;

		// Token: 0x04001BE6 RID: 7142
		private bool _isHoveringClearButton;

		// Token: 0x04001BE7 RID: 7143
		private bool _isPressingClearButton;

		// Token: 0x04001BE8 RID: 7144
		private InputFieldButtonStyle _clearButtonStyle;

		// Token: 0x04001BE9 RID: 7145
		private TexturePatch _clearButtonPatch;

		// Token: 0x04001BEA RID: 7146
		private Rectangle _clearButtonRectangle;

		// Token: 0x04001BEB RID: 7147
		[UIMarkupProperty]
		public char? PasswordChar;

		// Token: 0x04001BEC RID: 7148
		[UIMarkupProperty]
		public bool IsReadOnly;

		// Token: 0x04001BED RID: 7149
		[UIMarkupProperty]
		public int MaxLength = 255;

		// Token: 0x04001BEE RID: 7150
		public Action RightClicking;

		// Token: 0x04001BEF RID: 7151
		private int _mouseClickCount;

		// Token: 0x04001BF0 RID: 7152
		public Action<SDL.SDL_Keycode> KeyDown;

		// Token: 0x04001BF1 RID: 7153
		public Action Validating;

		// Token: 0x04001BF2 RID: 7154
		public Action Dismissing;

		// Token: 0x04001BF3 RID: 7155
		public Action Blurred;

		// Token: 0x04001BF4 RID: 7156
		public Action Focused;

		// Token: 0x04001BF5 RID: 7157
		private int _scrollOffset;

		// Token: 0x04001BF6 RID: 7158
		protected string _text = "";

		// Token: 0x04001BF7 RID: 7159
		private int _cursorIndex;

		// Token: 0x04001BF8 RID: 7160
		private int _relativeSelectionOffset;

		// Token: 0x04001BF9 RID: 7161
		protected bool _isFocused;

		// Token: 0x04001BFA RID: 7162
		private float _cursorTimer;

		// Token: 0x04001BFB RID: 7163
		private Font _font;

		// Token: 0x04001BFC RID: 7164
		private Font _placeholderFont;

		// Token: 0x02000D2E RID: 3374
		private enum CharType
		{
			// Token: 0x04004106 RID: 16646
			AlphaNumeric,
			// Token: 0x04004107 RID: 16647
			Whitespace,
			// Token: 0x04004108 RID: 16648
			Other
		}
	}
}
