using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000868 RID: 2152
	[UIMarkupElement]
	public class MultilineTextField : InputElement<string>
	{
		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06003C7F RID: 15487 RVA: 0x000963B6 File Offset: 0x000945B6
		// (set) Token: 0x06003C80 RID: 15488 RVA: 0x000963BE File Offset: 0x000945BE
		private int LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._cursorTimer = 0f;
				this._lineNumber = MathHelper.Clamp(value, 0, this._lineInfo.Count - 1);
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06003C81 RID: 15489 RVA: 0x000963E6 File Offset: 0x000945E6
		private int LineIndex
		{
			get
			{
				return this._lineInfo[this.LineNumber].LineIndex;
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06003C82 RID: 15490 RVA: 0x000963FE File Offset: 0x000945FE
		// (set) Token: 0x06003C83 RID: 15491 RVA: 0x00096406 File Offset: 0x00094606
		private int CursorIndex
		{
			get
			{
				return this._cursorIndex;
			}
			set
			{
				this._cursorTimer = 0f;
				this._cursorIndex = MathHelper.Clamp(value, 0, this.CurrentLine.Length);
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06003C84 RID: 15492 RVA: 0x0009642C File Offset: 0x0009462C
		// (set) Token: 0x06003C85 RID: 15493 RVA: 0x00096434 File Offset: 0x00094634
		public List<string> Lines { get; private set; } = new List<string>
		{
			""
		};

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06003C86 RID: 15494 RVA: 0x0009643D File Offset: 0x0009463D
		// (set) Token: 0x06003C87 RID: 15495 RVA: 0x00096450 File Offset: 0x00094650
		private string CurrentLine
		{
			get
			{
				return this.Lines[this.LineIndex];
			}
			set
			{
				this.Lines[this.LineIndex] = value;
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x00096465 File Offset: 0x00094665
		// (set) Token: 0x06003C89 RID: 15497 RVA: 0x00096478 File Offset: 0x00094678
		public override string Value
		{
			get
			{
				return string.Join(Environment.NewLine, this.Lines);
			}
			set
			{
				this.Lines = Enumerable.ToList<string>(value.Split(MultilineTextField.newlineChars, StringSplitOptions.None));
				this._selection = null;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.UpdateLineInfo();
					this.MoveCursorToEnd();
				}
			}
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x000964C0 File Offset: 0x000946C0
		public MultilineTextField(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00096540 File Offset: 0x00094740
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			bool flag = this.AutoFocus && this.Desktop.FocusedElement == null;
			if (flag)
			{
				this.Desktop.FocusElement(this, true);
				bool flag2 = this.GetLength() != 0 && this.AutoSelectAll;
				if (flag2)
				{
					this.SelectAll();
				}
			}
			this.LayoutParentForAutoGrow();
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000965B8 File Offset: 0x000947B8
		protected override void OnUnmounted()
		{
			bool isFocused = this._isFocused;
			if (isFocused)
			{
				this.Desktop.FocusElement(null, true);
			}
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x000965F7 File Offset: 0x000947F7
		protected virtual void Animate(float deltaTime)
		{
			this._cursorTimer += deltaTime;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x00096608 File Offset: 0x00094808
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

		// Token: 0x06003C8F RID: 15503 RVA: 0x00096640 File Offset: 0x00094840
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

		// Token: 0x06003C90 RID: 15504 RVA: 0x00096678 File Offset: 0x00094878
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0009668C File Offset: 0x0009488C
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
				}
				else
				{
					InputFieldDecorationStyleState default4 = this.Decoration.Default;
					this.Background = ((default4 != null) ? default4.Background : null);
					InputFieldDecorationStyleState default5 = this.Decoration.Default;
					this.OutlineColor = (((default5 != null) ? default5.OutlineColor : null) ?? UInt32Color.White);
					InputFieldDecorationStyleState default6 = this.Decoration.Default;
					this.OutlineSize = (float)((default6 != null) ? default6.OutlineSize : null).GetValueOrDefault();
				}
			}
			this._font = this.Desktop.Provider.GetFontFamily(this.Style.FontName.Value).RegularFont;
			this._placeholderFont = this.Desktop.Provider.GetFontFamily(this.PlaceholderStyle.FontName.Value).RegularFont;
			this._fontSizeInPixels = this.Style.FontSize * this.Desktop.Scale;
			this._fontScale = this._fontSizeInPixels / (float)this._font.BaseSize;
			this._scaledLineHeight = this.Desktop.ScaleRound((float)this._font.Height * this.Style.FontSize / (float)this._font.BaseSize);
			base.ApplyStyles();
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x00096924 File Offset: 0x00094B24
		protected override void LayoutSelf()
		{
			this.UpdateLineInfo();
			this._textAreaHeight = this._scaledLineHeight * this.GetTotalVisibleLines();
			this._textAreaLeft = this._rectangleAfterPadding.X;
			this._textAreaTop = this._rectangleAfterPadding.Center.Y - (int)((float)this._textAreaHeight / 2f);
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00096984 File Offset: 0x00094B84
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			this.ApplyStyles();
			Point result = base.ComputeScaledMinSize(maxWidth, maxHeight);
			bool flag = this.Anchor.Height == null && this.FlexWeight == 0;
			if (flag)
			{
				this.UpdateLineInfo();
				int num = (this.Padding.Vertical != null) ? this.Desktop.ScaleRound((float)this.Padding.Vertical.Value) : 0;
				result.Y = this._scaledLineHeight * this.GetTotalVisibleLines() + num;
			}
			return result;
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00096A24 File Offset: 0x00094C24
		protected override float? GetScaledHeight()
		{
			float? scaledHeight = base.GetScaledHeight();
			bool flag = scaledHeight != null;
			float? result;
			if (flag)
			{
				result = scaledHeight;
			}
			else
			{
				int num = (this.Padding.Vertical != null) ? this.Desktop.ScaleRound((float)this.Padding.Vertical.Value) : 0;
				result = new float?((float)(this._scaledLineHeight * this.GetTotalVisibleLines() + num));
			}
			return result;
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x00096A9B File Offset: 0x00094C9B
		private int GetLength()
		{
			return Enumerable.Sum<string>(this.Lines, (string l) => l.Length);
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x00096AC7 File Offset: 0x00094CC7
		private int GetTotalOccupiedLines()
		{
			return this._lineInfo.Count;
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x00096AD4 File Offset: 0x00094CD4
		private int GetTotalVisibleLines()
		{
			return this.AutoGrow ? this.GetTotalOccupiedLines() : (this._numLineWraps + this.MaxLines);
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00096AF3 File Offset: 0x00094CF3
		protected override void OnMouseEnter()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.IBeam);
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00096B0B File Offset: 0x00094D0B
		protected override void OnMouseLeave()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			this.ApplyStyles();
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00096B2C File Offset: 0x00094D2C
		protected override void OnMouseMove()
		{
			bool flag = !this._isLeftMouseButtonHeld;
			if (!flag)
			{
				this.LineNumber = this.GetLineNumberAtPosition(this.Desktop.MousePosition.Y);
				this.CursorIndex = this.GetAbsoluteCursorIndexAtPosition(this.Desktop.MousePosition);
				this.UpdateSelection();
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00096B88 File Offset: 0x00094D88
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this._cursorTimer = 0f;
			this._isLeftMouseButtonHeld = ((long)evt.Button == 1L);
			this._numMouseClicks = evt.Clicks;
			bool flag = evt.Clicks != 1;
			if (!flag)
			{
				this.Desktop.FocusElement(this, true);
				this.LineNumber = this.GetLineNumberAtPosition(this.Desktop.MousePosition.Y);
				this.CursorIndex = this.GetAbsoluteCursorIndexAtPosition(this.Desktop.MousePosition);
				bool flag2 = !this._isLeftMouseButtonHeld;
				if (!flag2)
				{
					this._selectionLineNumber = this.LineNumber;
					this._selectionCursorIndex = this.CursorIndex;
				}
			}
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x00096C3C File Offset: 0x00094E3C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				this._isLeftMouseButtonHeld = false;
				switch (this._numMouseClicks)
				{
				case 1:
					this.UpdateSelection();
					break;
				case 2:
				{
					bool isFocused = this._isFocused;
					if (isFocused)
					{
						this.SelectWordAt(this.GetAbsoluteCursorIndexAtPosition(this.Desktop.MousePosition));
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

		// Token: 0x06003C9D RID: 15517 RVA: 0x00096CCC File Offset: 0x00094ECC
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			MultilineTextField.<>c__DisplayClass70_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			bool isShortcutKeyDown = this.Desktop.IsShortcutKeyDown;
			CS$<>8__locals1.isShiftDown = this.Desktop.IsShiftKeyDown;
			bool isWordSkipDown = this.Desktop.IsWordSkipDown;
			bool isLineSkipDown = this.Desktop.IsLineSkipDown;
			if (keycode <= SDL.SDL_Keycode.SDLK_c)
			{
				if (keycode <= SDL.SDL_Keycode.SDLK_RETURN)
				{
					if (keycode != SDL.SDL_Keycode.SDLK_BACKSPACE)
					{
						if (keycode == SDL.SDL_Keycode.SDLK_RETURN)
						{
							bool flag = (this.MaxLines > 0 && this.Lines.Count == this.MaxLines) || this.IsReadOnly;
							if (!flag)
							{
								this.DeleteSelection();
								bool flag2 = this.CursorIndex < this.CurrentLine.Length;
								if (flag2)
								{
									string item = this.CurrentLine.Substring(0, this.CursorIndex);
									string currentLine = this.CurrentLine.Substring(this.CursorIndex, this.CurrentLine.Length - this.CursorIndex);
									this.CurrentLine = currentLine;
									this.Lines.Insert(this.LineIndex, item);
								}
								else
								{
									this.Lines.Insert(this.LineIndex + 1, "");
								}
								this.OnValueChanged();
								int num = this.LineNumber;
								this.LineNumber = num + 1;
								this.CursorIndex = 0;
							}
						}
					}
					else
					{
						bool isReadOnly = this.IsReadOnly;
						if (!isReadOnly)
						{
							bool flag3 = !this.DeleteSelection();
							if (flag3)
							{
								bool flag4 = this.CursorIndex > 0;
								if (flag4)
								{
									int num2 = this.CursorIndex;
									bool flag5 = isWordSkipDown;
									if (flag5)
									{
										num2 = this.CurrentLine.LastIndexOf(' ', Math.Max(num2 - 2, 0)) + 1;
									}
									else
									{
										num2--;
									}
									bool flag6 = this.LineNumber >= this._lineInfo.Count || this.CursorIndex == this._lineInfo[this.LineNumber].StartAt;
									if (flag6)
									{
										int num = this.LineNumber;
										this.LineNumber = num - 1;
									}
									this.CurrentLine = this.CurrentLine.Remove(num2, this.CursorIndex - num2);
									this.CursorIndex = num2;
								}
								else
								{
									bool flag7 = this.LineNumber > 0;
									if (flag7)
									{
										string currentLine2 = this.CurrentLine;
										this.Lines.RemoveAt(this.LineIndex);
										int num = this.LineNumber;
										this.LineNumber = num - 1;
										this.CursorIndex = this.CurrentLine.Length;
										bool flag8 = currentLine2.Length > 0;
										if (flag8)
										{
											this.CurrentLine += currentLine2;
										}
									}
								}
							}
							this.OnValueChanged();
						}
					}
				}
				else if (keycode != SDL.SDL_Keycode.SDLK_a)
				{
					if (keycode == SDL.SDL_Keycode.SDLK_c)
					{
						bool flag9 = isShortcutKeyDown;
						if (flag9)
						{
							this.CopySelectionToClipboard();
						}
					}
				}
				else
				{
					bool flag10 = isShortcutKeyDown;
					if (flag10)
					{
						this.SelectAll();
					}
				}
			}
			else if (keycode <= SDL.SDL_Keycode.SDLK_x)
			{
				if (keycode != SDL.SDL_Keycode.SDLK_v)
				{
					if (keycode == SDL.SDL_Keycode.SDLK_x)
					{
						bool flag11 = !isShortcutKeyDown;
						if (!flag11)
						{
							this.CopySelectionToClipboard();
							bool flag12 = this.DeleteSelection();
							if (flag12)
							{
								this.OnValueChanged();
							}
						}
					}
				}
				else
				{
					bool flag13 = isShortcutKeyDown;
					if (flag13)
					{
						this.PasteFromClipboard();
					}
				}
			}
			else if (keycode != SDL.SDL_Keycode.SDLK_DELETE)
			{
				switch (keycode)
				{
				case SDL.SDL_Keycode.SDLK_HOME:
					this.<OnKeyDown>g__SkipToLineStart|70_0(ref CS$<>8__locals1);
					break;
				case SDL.SDL_Keycode.SDLK_END:
					this.<OnKeyDown>g__SkipToLineEnd|70_1(ref CS$<>8__locals1);
					break;
				case SDL.SDL_Keycode.SDLK_RIGHT:
				{
					bool flag14 = isLineSkipDown;
					if (flag14)
					{
						this.<OnKeyDown>g__SkipToLineEnd|70_1(ref CS$<>8__locals1);
					}
					else
					{
						bool flag15 = this.CursorIndex == this.CurrentLine.Length && this.LineNumber < this.GetTotalOccupiedLines() - 1;
						if (flag15)
						{
							int num = this.LineNumber;
							this.LineNumber = num + 1;
							this.CursorIndex = 0;
						}
						bool flag16 = this.CursorIndex < this.CurrentLine.Length;
						if (flag16)
						{
							bool flag17 = isWordSkipDown;
							if (flag17)
							{
								this.CursorIndex = this.CurrentLine.IndexOf(' ', this.CursorIndex, this.CurrentLine.Length - this.CursorIndex) + 1;
								bool flag18 = this.CursorIndex == 0;
								if (flag18)
								{
									this.CursorIndex = this.CurrentLine.Length;
								}
							}
							else
							{
								int num = this.CursorIndex;
								this.CursorIndex = num + 1;
							}
						}
						bool flag19 = this.CursorIndex > this._lineInfo[this.LineNumber].EndAt;
						if (flag19)
						{
							int num = this.LineNumber;
							this.LineNumber = num + 1;
						}
						bool isShiftDown = CS$<>8__locals1.isShiftDown;
						if (isShiftDown)
						{
							this.UpdateSelection();
						}
						else
						{
							this._selection = null;
							this._selectionLineNumber = this.LineNumber;
							this._selectionCursorIndex = this.CursorIndex;
						}
					}
					break;
				}
				case SDL.SDL_Keycode.SDLK_LEFT:
				{
					bool flag20 = isLineSkipDown;
					if (flag20)
					{
						this.<OnKeyDown>g__SkipToLineStart|70_0(ref CS$<>8__locals1);
					}
					else
					{
						bool flag21 = this.CursorIndex == 0 && this.LineNumber > 0;
						if (flag21)
						{
							int num = this.LineNumber;
							this.LineNumber = num - 1;
							this.CursorIndex = this.CurrentLine.Length;
						}
						bool flag22 = this.CursorIndex > 0;
						if (flag22)
						{
							bool flag23 = isWordSkipDown;
							if (flag23)
							{
								this.CursorIndex = this.CurrentLine.LastIndexOf(' ', Math.Max(this.CursorIndex - 2, 0)) + 1;
							}
							else
							{
								int num = this.CursorIndex;
								this.CursorIndex = num - 1;
							}
						}
						bool flag24 = this.CursorIndex < this._lineInfo[this.LineNumber].StartAt;
						if (flag24)
						{
							int num = this.LineNumber;
							this.LineNumber = num - 1;
						}
						bool isShiftDown2 = CS$<>8__locals1.isShiftDown;
						if (isShiftDown2)
						{
							this.UpdateSelection();
						}
						else
						{
							this._selection = null;
							this._selectionLineNumber = this.LineNumber;
							this._selectionCursorIndex = this.CursorIndex;
						}
					}
					break;
				}
				case SDL.SDL_Keycode.SDLK_DOWN:
				{
					bool flag25 = this.LineNumber == this.GetTotalOccupiedLines() - 1;
					if (flag25)
					{
						this.CursorIndex = this.CurrentLine.Length;
					}
					else
					{
						this.<OnKeyDown>g__MoveCursorVertically|70_2(1, ref CS$<>8__locals1);
					}
					bool isShiftDown3 = CS$<>8__locals1.isShiftDown;
					if (isShiftDown3)
					{
						this.UpdateSelection();
					}
					else
					{
						this._selection = null;
						this._selectionLineNumber = this.LineNumber;
						this._selectionCursorIndex = this.CursorIndex;
					}
					break;
				}
				case SDL.SDL_Keycode.SDLK_UP:
				{
					bool flag26 = this.LineNumber == 0;
					if (flag26)
					{
						this.CursorIndex = 0;
					}
					else
					{
						this.<OnKeyDown>g__MoveCursorVertically|70_2(-1, ref CS$<>8__locals1);
					}
					bool isShiftDown4 = CS$<>8__locals1.isShiftDown;
					if (isShiftDown4)
					{
						this.UpdateSelection();
					}
					else
					{
						this._selection = null;
						this._selectionLineNumber = this.LineNumber;
						this._selectionCursorIndex = this.CursorIndex;
					}
					break;
				}
				}
			}
			else
			{
				bool isReadOnly2 = this.IsReadOnly;
				if (!isReadOnly2)
				{
					bool flag27 = !this.DeleteSelection() && this.LineNumber < this.GetTotalOccupiedLines();
					if (flag27)
					{
						bool flag28 = this.CursorIndex == this.CurrentLine.Length && this.Lines.Count > this.LineIndex + 1;
						if (flag28)
						{
							string str = this.Lines[this.LineIndex + 1];
							this.Lines.RemoveAt(this.LineIndex + 1);
							this.CurrentLine += str;
						}
						else
						{
							bool flag29 = this.CursorIndex < this.CurrentLine.Length;
							if (flag29)
							{
								string currentLine3 = this.CurrentLine;
								int num3 = this.CursorIndex;
								bool flag30 = isWordSkipDown;
								if (flag30)
								{
									num3 = currentLine3.IndexOf(' ', num3, currentLine3.Length - num3) + 1;
									bool flag31 = num3 == 0;
									if (flag31)
									{
										num3 = currentLine3.Length;
									}
								}
								else
								{
									num3++;
								}
								this.CurrentLine = currentLine3.Remove(this.CursorIndex, num3 - this.CursorIndex);
							}
						}
					}
					this.OnValueChanged();
				}
			}
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x00097514 File Offset: 0x00095714
		private bool TextOverflowsMaxLength(string text)
		{
			string text2 = string.Join("", this.Lines);
			return this.MaxLength > 0 && text2.Length + text.Length > this.MaxLength && this._selection == null;
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x00097564 File Offset: 0x00095764
		protected internal override void OnTextInput(string text)
		{
			bool flag = this.IsReadOnly || this.TextOverflowsMaxLength(text);
			if (!flag)
			{
				this.DeleteSelection();
				this._selection = null;
				bool flag2 = this.Lines.Count == 0;
				if (flag2)
				{
					this.Lines.Add("");
				}
				this.CurrentLine = this.CurrentLine.Insert(this.CursorIndex, text);
				this.CursorIndex += text.Length;
				this.OnValueChanged();
			}
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x000975F0 File Offset: 0x000957F0
		private void OnValueChanged()
		{
			this.UpdateLineInfo();
			bool flag = this.LineNumber >= this._lineInfo.Count || this.CursorIndex < this._lineInfo[this.LineNumber].StartAt;
			if (flag)
			{
				int lineNumber = this.LineNumber;
				this.LineNumber = lineNumber - 1;
			}
			else
			{
				bool flag2 = this.CursorIndex > this._lineInfo[this.LineNumber].EndAt;
				if (flag2)
				{
					int lineNumber = this.LineNumber;
					this.LineNumber = lineNumber + 1;
				}
			}
			Action valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged();
			}
			this.LayoutParentForAutoGrow();
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x000976A0 File Offset: 0x000958A0
		private void UpdateLineInfo()
		{
			int x = this._rectangleAfterPadding.X + this._rectangleAfterPadding.Width;
			this._numLineWraps = 0;
			this._lineInfo.Clear();
			for (int i = 0; i < this.Lines.Count; i++)
			{
				string text = this.Lines[i];
				int num = 0;
				int num2 = this.GetRelativeCursorIndexAtPosition(text, x);
				while (text.Length > num2)
				{
					int num3 = text.Substring(num, num2 - num).LastIndexOf(' ');
					bool flag = num3 > 0;
					if (flag)
					{
						num2 = num + num3 + 1;
					}
					this._numLineWraps++;
					this._lineInfo.Add(new MultilineTextField.LineInfo
					{
						LineIndex = i,
						StartAt = num,
						EndAt = num2
					});
					num = num2;
					num2 = num + this.GetRelativeCursorIndexAtPosition(text.Substring(num), x);
				}
				this._lineInfo.Add(new MultilineTextField.LineInfo
				{
					LineIndex = i,
					StartAt = num,
					EndAt = num2
				});
			}
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x000977D8 File Offset: 0x000959D8
		private void SelectWordAt(int cursorIndex)
		{
			int selectionCursorIndex = 0;
			int num = 0;
			bool flag = false;
			MultilineTextField.CharType charType = MultilineTextField.CharType.Other;
			for (int i = 0; i < this.CurrentLine.Length; i++)
			{
				char c = this.CurrentLine[i];
				bool flag2 = char.IsWhiteSpace(c);
				MultilineTextField.CharType charType2;
				if (flag2)
				{
					charType2 = MultilineTextField.CharType.Whitespace;
				}
				else
				{
					bool flag3 = char.IsLetter(c) || char.IsNumber(c);
					if (flag3)
					{
						charType2 = MultilineTextField.CharType.AlphaNumeric;
					}
					else
					{
						charType2 = MultilineTextField.CharType.Other;
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
					selectionCursorIndex = i;
				}
				num = i;
				bool flag6 = i != cursorIndex;
				if (!flag6)
				{
					flag = true;
					bool flag7 = charType2 != MultilineTextField.CharType.Other;
					if (!flag7)
					{
						selectionCursorIndex = i;
						break;
					}
				}
			}
			this._selectionLineNumber = this.LineNumber;
			this._selectionCursorIndex = selectionCursorIndex;
			this.CursorIndex = num + 1;
			this.UpdateSelection();
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x000978C4 File Offset: 0x00095AC4
		private void SelectAll()
		{
			this._selectionLineNumber = 0;
			this._selectionCursorIndex = 0;
			this.MoveCursorToEnd();
			this.UpdateSelection();
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x000978E3 File Offset: 0x00095AE3
		private void MoveCursorToEnd()
		{
			this.LineNumber = this.GetTotalOccupiedLines() - 1;
			this.CursorIndex = this.CurrentLine.Length;
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00097908 File Offset: 0x00095B08
		private void CopySelectionToClipboard()
		{
			bool flag = this._selection == null;
			if (!flag)
			{
				string text = "";
				int num = this._lineInfo[this._selection.StartLineNumber].LineIndex;
				for (int i = this._selection.StartLineNumber; i <= this._selection.EndLineNumber; i++)
				{
					int lineIndex = this._lineInfo[i].LineIndex;
					int startAt = this._lineInfo[i].StartAt;
					int endAt = this._lineInfo[i].EndAt;
					int num2 = (i == this._selection.StartLineNumber) ? (this._selection.StartCursorIndex - startAt) : startAt;
					int num3 = (i == this._selection.EndLineNumber) ? this._selection.EndCursorIndex : endAt;
					bool flag2 = lineIndex != num;
					if (flag2)
					{
						text += Environment.NewLine;
					}
					text += this.Lines[lineIndex].Substring(num2, num3 - num2);
					num = lineIndex;
				}
				SDL.SDL_SetClipboardText(text);
			}
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x00097A3C File Offset: 0x00095C3C
		private void PasteFromClipboard()
		{
			string text = SDL.SDL_GetClipboardText();
			bool flag = text == null || (!this.DeleteSelection() && this.GetLength() == this.MaxLength);
			if (!flag)
			{
				List<string> list = Enumerable.ToList<string>(text.Split(MultilineTextField.newlineChars, StringSplitOptions.None));
				int num = (this.MaxLines > 0 && this.Lines.Count + list.Count > this.MaxLines) ? (this.MaxLines - (this.Lines.Count - 1)) : list.Count;
				string text2 = (this.CursorIndex == this._lineInfo[this.LineNumber].EndAt) ? "" : this.CurrentLine.Substring(this.CursorIndex);
				this.CurrentLine = this.CurrentLine.Substring(0, this.CursorIndex);
				for (int i = 0; i < num; i++)
				{
					string text3 = list[i];
					bool flag2 = this.TextOverflowsMaxLength(text3 + text2);
					bool flag3 = flag2;
					if (flag3)
					{
						int num2 = this.MaxLength - this.GetLength() - text2.Length;
						text3 = ((num2 > 0) ? text3.Substring(0, num2) : "");
					}
					bool flag4 = i == 0;
					if (flag4)
					{
						this.CurrentLine = this.CurrentLine.Insert(this.CursorIndex, text3);
						this.OnValueChanged();
						this.CursorIndex += text3.Length;
					}
					else
					{
						this.Lines.Insert(this.LineIndex + 1, text3);
						this.OnValueChanged();
						int lineNumber = this.LineNumber;
						this.LineNumber = lineNumber + 1;
						this.CursorIndex = this.CurrentLine.Length;
					}
					bool flag5 = i == num - 1;
					if (flag5)
					{
						this.CurrentLine += text2;
						this.OnValueChanged();
					}
					bool flag6 = flag2;
					if (flag6)
					{
						break;
					}
				}
				this._selection = null;
			}
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00097C50 File Offset: 0x00095E50
		private bool DeleteSelection()
		{
			bool flag = this._selection == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int lineIndex = this._lineInfo[this._selection.StartLineNumber].LineIndex;
				int lineIndex2 = this._lineInfo[this._selection.EndLineNumber].LineIndex;
				string text = this.Lines[lineIndex];
				string text2 = this.Lines[lineIndex2];
				string str = (this._selection.StartCursorIndex > 0) ? text.Substring(0, this._selection.StartCursorIndex) : "";
				string str2 = (this._selection.EndCursorIndex < text2.Length) ? text2.Substring(this._selection.EndCursorIndex) : "";
				this.Lines[lineIndex] = str + str2;
				for (int i = lineIndex2; i > lineIndex; i--)
				{
					this.Lines.RemoveAt(i);
				}
				this.LineNumber = this._selection.StartLineNumber;
				this.CursorIndex = this._selection.StartCursorIndex;
				this.UpdateLineInfo();
				this._selection = null;
				result = true;
			}
			return result;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00097D90 File Offset: 0x00095F90
		private void UpdateSelection()
		{
			bool flag = this._selectionLineNumber == this.LineNumber && this._selectionCursorIndex == this.CursorIndex;
			if (flag)
			{
				this._selection = null;
			}
			else
			{
				this._selection = new MultilineTextField.SelectionInfo(this._selectionLineNumber, this.LineNumber, this._selectionCursorIndex, this.CursorIndex);
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00097DF0 File Offset: 0x00095FF0
		private int GetLineNumberAtPosition(int y)
		{
			int num = y - this._rectangleAfterPadding.Y;
			return Math.Min(Math.Max(0, num / this._scaledLineHeight), this.GetTotalOccupiedLines() - 1);
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00097E2C File Offset: 0x0009602C
		private int GetAbsoluteCursorIndexAtPosition(Point pos)
		{
			int lineNumberAtPosition = this.GetLineNumberAtPosition(pos.Y);
			int lineIndex = this.LineIndex;
			int startAt = this._lineInfo[lineNumberAtPosition].StartAt;
			int endAt = this._lineInfo[lineNumberAtPosition].EndAt;
			int relativeCursorIndexAtPosition = this.GetRelativeCursorIndexAtPosition(this.Lines[lineIndex].Substring(startAt, endAt - startAt), pos.X);
			return startAt + relativeCursorIndexAtPosition;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00097EA0 File Offset: 0x000960A0
		private int GetRelativeCursorIndexAtPosition(string line, int x)
		{
			float num = (float)(x - this._rectangleAfterPadding.X) / this._fontScale;
			float num2 = 0f;
			int num3 = 0;
			foreach (ushort key in line)
			{
				float num4;
				bool flag = !this._font.GlyphAdvances.TryGetValue(key, out num4);
				if (!flag)
				{
					bool flag2 = num <= num2 + num4 / 2f;
					if (flag2)
					{
						break;
					}
					num2 += num4;
					num3++;
				}
			}
			return num3;
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00097F38 File Offset: 0x00096138
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
			bool flag = this.GetLength() == 0 && this._lineInfo.Count == 1 && this.PlaceholderText != null;
			if (flag)
			{
				float size = this.PlaceholderStyle.FontSize * this.Desktop.Scale;
				this.Desktop.Batcher2D.RequestDrawText(this._placeholderFont, size, this.PlaceholderStyle.RenderUppercase ? this.PlaceholderText.ToUpperInvariant() : this.PlaceholderText, new Vector3((float)this._rectangleAfterPadding.X, (float)this._textAreaTop, 0f), this.PlaceholderStyle.TextColor, this.PlaceholderStyle.RenderBold, this.PlaceholderStyle.RenderItalics, 0f);
			}
			for (int i = 0; i < this._lineInfo.Count; i++)
			{
				int startAt = this._lineInfo[i].StartAt;
				int endAt = this._lineInfo[i].EndAt;
				string text = this.Lines[this._lineInfo[i].LineIndex].Substring(startAt, endAt - startAt);
				bool flag2 = text.Length > 0;
				if (flag2)
				{
					this.Desktop.Batcher2D.RequestDrawText(this._font, this._fontSizeInPixels, text, new Vector3((float)this._textAreaLeft, (float)(this._textAreaTop + i * this._scaledLineHeight), 0f), this.Style.TextColor, this.Style.RenderBold, this.Style.RenderItalics, 0f);
				}
			}
			bool flag3 = this._selection != null;
			if (flag3)
			{
				for (int j = this._selection.StartLineNumber; j <= this._selection.EndLineNumber; j++)
				{
					int startAt2 = this._lineInfo[j].StartAt;
					int endAt2 = this._lineInfo[j].EndAt;
					string text2 = this.Lines[this._lineInfo[j].LineIndex].Substring(startAt2, endAt2 - startAt2);
					int num = 0;
					bool flag4 = j == this._selection.StartLineNumber && this._selection.StartsBetween(startAt2, endAt2);
					if (flag4)
					{
						num = this.<PrepareForDrawSelf>g__GetHorizontalPositionAt|85_0(text2, this._selection.StartCursorIndex - startAt2);
					}
					bool flag5 = j == this._selection.EndLineNumber && this._selection.EndsBetween(startAt2, endAt2);
					int charIndex;
					if (flag5)
					{
						charIndex = this._selection.EndCursorIndex - startAt2;
					}
					else
					{
						charIndex = text2.Length;
					}
					int num2 = 0;
					bool flag6 = text2.Length > 0;
					if (flag6)
					{
						num2 = this.<PrepareForDrawSelf>g__GetHorizontalPositionAt|85_0(text2, charIndex);
					}
					else
					{
						bool flag7 = j != this.LineNumber || (j == this.LineNumber && j != this.CursorIndex);
						if (flag7)
						{
							num2 = 5;
						}
					}
					int y = this._textAreaTop + j * this._scaledLineHeight;
					this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, new Rectangle(this._textAreaLeft + num, y, num2 - num, this._scaledLineHeight), UInt32Color.FromRGBA(this._isFocused ? 4294967136U : 4294967072U));
				}
			}
			bool flag8 = this._isFocused && this.Desktop.IsFocused && this._cursorTimer % 1f < 0.5f;
			if (flag8)
			{
				int startAt3 = this._lineInfo[this.LineNumber].StartAt;
				int endAt3 = this._lineInfo[this.LineNumber].EndAt;
				string line = this.Lines[this._lineInfo[this.LineNumber].LineIndex].Substring(startAt3, endAt3 - startAt3);
				int num3 = this.<PrepareForDrawSelf>g__GetHorizontalPositionAt|85_0(line, this.CursorIndex - startAt3);
				int y2 = this._textAreaTop + this.LineNumber * this._scaledLineHeight;
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, new Rectangle(this._textAreaLeft + num3, y2, 1, this._scaledLineHeight), this.Style.TextColor);
			}
			this.Desktop.Batcher2D.PopScissor();
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00098438 File Offset: 0x00096638
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

		// Token: 0x06003CAE RID: 15534 RVA: 0x00098474 File Offset: 0x00096674
		private void LayoutParentForAutoGrow()
		{
			Element element = this;
			while (element.Parent != null && element.Parent.LayoutMode > LayoutMode.Full)
			{
				element = element.Parent;
			}
			element.Layout(null, true);
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x000984E4 File Offset: 0x000966E4
		[CompilerGenerated]
		private void <OnKeyDown>g__SkipToLineStart|70_0(ref MultilineTextField.<>c__DisplayClass70_0 A_1)
		{
			bool isShiftDown = A_1.isShiftDown;
			if (isShiftDown)
			{
				this._selectionLineNumber = this.LineNumber;
				this._selectionCursorIndex = this.CursorIndex;
			}
			this.CursorIndex = 0;
			this.UpdateSelection();
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00098528 File Offset: 0x00096728
		[CompilerGenerated]
		private void <OnKeyDown>g__SkipToLineEnd|70_1(ref MultilineTextField.<>c__DisplayClass70_0 A_1)
		{
			bool isShiftDown = A_1.isShiftDown;
			if (isShiftDown)
			{
				this._selectionLineNumber = this.LineNumber;
				this._selectionCursorIndex = this.CursorIndex;
			}
			this.CursorIndex = this._lineInfo[this.LineNumber].EndAt;
			this.UpdateSelection();
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00098580 File Offset: 0x00096780
		[CompilerGenerated]
		private void <OnKeyDown>g__MoveCursorVertically|70_2(int offset, ref MultilineTextField.<>c__DisplayClass70_0 A_2)
		{
			bool flag = !A_2.isShiftDown && this._selection != null;
			if (flag)
			{
				this.LineNumber = this._selectionLineNumber;
				this.CursorIndex = this._selectionCursorIndex;
				this._selection = null;
			}
			else
			{
				bool flag2 = A_2.isShiftDown && this._selectionCursorIndex == -1;
				if (flag2)
				{
					this._selectionLineNumber = this.LineNumber;
					this._selectionCursorIndex = this.CursorIndex;
				}
			}
			int num = this.CursorIndex - this._lineInfo[this.LineNumber].StartAt;
			this.LineNumber += offset;
			this.CursorIndex = Math.Min(this._lineInfo[this.LineNumber].StartAt + num, this.CurrentLine.Length);
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x0009865C File Offset: 0x0009685C
		[CompilerGenerated]
		private int <PrepareForDrawSelf>g__GetHorizontalPositionAt|85_0(string line, int charIndex)
		{
			return (int)(this._font.CalculateTextWidth(line.Substring(0, charIndex)) * this._fontScale);
		}

		// Token: 0x04001C08 RID: 7176
		[UIMarkupProperty]
		public InputFieldStyle Style = new InputFieldStyle();

		// Token: 0x04001C09 RID: 7177
		[UIMarkupProperty]
		public InputFieldStyle PlaceholderStyle = new InputFieldStyle();

		// Token: 0x04001C0A RID: 7178
		[UIMarkupProperty]
		public InputFieldDecorationStyle Decoration;

		// Token: 0x04001C0B RID: 7179
		[UIMarkupProperty]
		public bool AutoFocus;

		// Token: 0x04001C0C RID: 7180
		[UIMarkupProperty]
		public bool AutoSelectAll;

		// Token: 0x04001C0D RID: 7181
		[UIMarkupProperty]
		public bool IsReadOnly;

		// Token: 0x04001C0E RID: 7182
		[UIMarkupProperty]
		public int MaxLength = 255;

		// Token: 0x04001C0F RID: 7183
		[UIMarkupProperty]
		public int MaxLines = 0;

		// Token: 0x04001C10 RID: 7184
		[UIMarkupProperty]
		public bool AutoGrow = true;

		// Token: 0x04001C11 RID: 7185
		[UIMarkupProperty]
		public string PlaceholderText;

		// Token: 0x04001C12 RID: 7186
		public Action Dismissing;

		// Token: 0x04001C13 RID: 7187
		public Action Blurred;

		// Token: 0x04001C14 RID: 7188
		public Action Focused;

		// Token: 0x04001C15 RID: 7189
		private float _cursorTimer;

		// Token: 0x04001C16 RID: 7190
		private int _lineNumber;

		// Token: 0x04001C17 RID: 7191
		private int _cursorIndex;

		// Token: 0x04001C19 RID: 7193
		private bool _isFocused;

		// Token: 0x04001C1A RID: 7194
		private Font _font;

		// Token: 0x04001C1B RID: 7195
		private Font _placeholderFont;

		// Token: 0x04001C1C RID: 7196
		private float _fontSizeInPixels;

		// Token: 0x04001C1D RID: 7197
		private float _fontScale;

		// Token: 0x04001C1E RID: 7198
		private int _textAreaHeight;

		// Token: 0x04001C1F RID: 7199
		private int _textAreaLeft;

		// Token: 0x04001C20 RID: 7200
		private int _textAreaTop;

		// Token: 0x04001C21 RID: 7201
		private int _scaledLineHeight;

		// Token: 0x04001C22 RID: 7202
		private bool _isLeftMouseButtonHeld;

		// Token: 0x04001C23 RID: 7203
		private int _numMouseClicks;

		// Token: 0x04001C24 RID: 7204
		private int _numLineWraps = 0;

		// Token: 0x04001C25 RID: 7205
		private readonly List<MultilineTextField.LineInfo> _lineInfo = new List<MultilineTextField.LineInfo>();

		// Token: 0x04001C26 RID: 7206
		private int _selectionLineNumber = -1;

		// Token: 0x04001C27 RID: 7207
		private int _selectionCursorIndex = -1;

		// Token: 0x04001C28 RID: 7208
		private MultilineTextField.SelectionInfo _selection;

		// Token: 0x04001C29 RID: 7209
		private static readonly string[] newlineChars = new string[]
		{
			"\r\n",
			"\r",
			"\n"
		};

		// Token: 0x02000D38 RID: 3384
		private enum CharType
		{
			// Token: 0x04004132 RID: 16690
			AlphaNumeric,
			// Token: 0x04004133 RID: 16691
			Whitespace,
			// Token: 0x04004134 RID: 16692
			Other
		}

		// Token: 0x02000D39 RID: 3385
		private struct LineInfo
		{
			// Token: 0x04004135 RID: 16693
			public int LineIndex;

			// Token: 0x04004136 RID: 16694
			public int StartAt;

			// Token: 0x04004137 RID: 16695
			public int EndAt;
		}

		// Token: 0x02000D3A RID: 3386
		private class SelectionInfo
		{
			// Token: 0x060064F1 RID: 25841 RVA: 0x002106BC File Offset: 0x0020E8BC
			public SelectionInfo(int selectionLineNumber, int lineNumber, int selectionCursorIndex, int cursorIndex)
			{
				this.StartLineNumber = Math.Min(selectionLineNumber, lineNumber);
				this.EndLineNumber = Math.Max(selectionLineNumber, lineNumber);
				this.LineCount = this.EndLineNumber - this.StartLineNumber + 1;
				bool flag = this.LineCount == 1;
				if (flag)
				{
					this.StartCursorIndex = Math.Min(selectionCursorIndex, cursorIndex);
					this.EndCursorIndex = Math.Max(selectionCursorIndex, cursorIndex);
				}
				else
				{
					bool flag2 = this.EndLineNumber == lineNumber;
					if (flag2)
					{
						this.StartCursorIndex = selectionCursorIndex;
						this.EndCursorIndex = cursorIndex;
					}
					else
					{
						this.StartCursorIndex = cursorIndex;
						this.EndCursorIndex = selectionCursorIndex;
					}
				}
			}

			// Token: 0x060064F2 RID: 25842 RVA: 0x0021075E File Offset: 0x0020E95E
			public bool StartsBetween(int left, int right)
			{
				return this.StartCursorIndex >= left && this.StartCursorIndex <= right;
			}

			// Token: 0x060064F3 RID: 25843 RVA: 0x00210778 File Offset: 0x0020E978
			public bool EndsBetween(int left, int right)
			{
				return this.EndCursorIndex >= left && this.EndCursorIndex <= right;
			}

			// Token: 0x04004138 RID: 16696
			public readonly int StartLineNumber;

			// Token: 0x04004139 RID: 16697
			public readonly int EndLineNumber;

			// Token: 0x0400413A RID: 16698
			public readonly int StartCursorIndex;

			// Token: 0x0400413B RID: 16699
			public readonly int EndCursorIndex;

			// Token: 0x0400413C RID: 16700
			public readonly int LineCount;
		}
	}
}
