using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000866 RID: 2150
	[UIMarkupElement]
	public class Label : Element
	{
		// Token: 0x17001065 RID: 4197
		// (set) Token: 0x06003C6A RID: 15466 RVA: 0x00094A34 File Offset: 0x00092C34
		[UIMarkupProperty]
		public string Text
		{
			set
			{
				this._textSpanPortions.Clear();
				this._textSpans.Clear();
				bool flag = !string.IsNullOrEmpty(value);
				if (flag)
				{
					this._textSpans.Add(new Label.LabelSpan
					{
						Text = value
					});
				}
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06003C6B RID: 15467 RVA: 0x00094A7F File Offset: 0x00092C7F
		// (set) Token: 0x06003C6C RID: 15468 RVA: 0x00094A8C File Offset: 0x00092C8C
		[UIMarkupProperty]
		public IList<Label.LabelSpan> TextSpans
		{
			get
			{
				return this._textSpans.AsReadOnly();
			}
			set
			{
				this._textSpans.Clear();
				this._textSpans.AddRange(value);
				this._textSpanPortions.Clear();
			}
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x00094AB4 File Offset: 0x00092CB4
		public Label(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x00094B04 File Offset: 0x00092D04
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position) || (this._linkSpanPortions.Count == 0 && (this._tagSpanPortions.Count == 0 || this.TagMouseEntered == null) && !this._hasTooltipText);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x00094B6C File Offset: 0x00092D6C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = this._linkSpanPortions.Count == 0;
			if (!flag)
			{
				Debug.Assert(this.LinkActivating != null);
				int x = this.Desktop.MousePosition.X - this._rectangleAfterPadding.X;
				int y = this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Y;
				foreach (Label.LabelSpanPortion labelSpanPortion in this._linkSpanPortions)
				{
					bool flag2 = this.IsMouseInSpanPortion(x, y, labelSpanPortion);
					if (flag2)
					{
						this.LinkActivating(labelSpanPortion.Span.Link);
						break;
					}
				}
			}
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x00094C4C File Offset: 0x00092E4C
		protected override void OnMouseMove()
		{
			bool flag = this._tagSpanPortions.Count == 0;
			if (!flag)
			{
				Debug.Assert(this.TagMouseEntered != null);
				int x = this.Desktop.MousePosition.X - this._rectangleAfterPadding.X;
				int y = this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Y;
				foreach (Label.LabelSpanPortion labelSpanPortion in this._tagSpanPortions)
				{
					bool flag2 = this.IsMouseInSpanPortion(x, y, labelSpanPortion);
					if (flag2)
					{
						bool flag3 = this._currentlyHoveredTag != labelSpanPortion;
						if (flag3)
						{
							labelSpanPortion.CenterPoint = new Point((int)((float)this._rectangleAfterPadding.X + labelSpanPortion.X + labelSpanPortion.Width / 2f), (int)((float)this._rectangleAfterPadding.Y + labelSpanPortion.Y + (float)(this.GetSpanFont(labelSpanPortion.Span).Height / 2)));
							this._currentlyHoveredTag = labelSpanPortion;
							this.TagMouseEntered(labelSpanPortion);
						}
						return;
					}
				}
				this._currentlyHoveredTag = null;
				this.TagMouseEntered(null);
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00094DB4 File Offset: 0x00092FB4
		protected override void OnMouseLeave()
		{
			this._currentlyHoveredTag = null;
			Action<Label.LabelSpanPortion> tagMouseEntered = this.TagMouseEntered;
			if (tagMouseEntered != null)
			{
				tagMouseEntered(null);
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x00094DD4 File Offset: 0x00092FD4
		private bool IsMouseInSpanPortion(int x, int y, Label.LabelSpanPortion portion)
		{
			return (float)x >= portion.X && (float)x < portion.X + portion.Width && (float)y >= portion.Y && (float)y < portion.Y + (float)this._scaledLineHeight;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x00094E20 File Offset: 0x00093020
		private Font GetSpanFont(Label.LabelSpan span)
		{
			return (span.IsBold || this.Style.RenderBold) ? this._fontFamily.BoldFont : this._fontFamily.RegularFont;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x00094E50 File Offset: 0x00093050
		public override Point ComputeScaledMinSize(int? containerMaxWidth, int? containerMaxHeight)
		{
			Label.<>c__DisplayClass24_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.ApplyStyles();
			int num = this.Padding.Left.GetValueOrDefault() + this.Padding.Right.GetValueOrDefault();
			CS$<>8__locals1.maxWidth = containerMaxWidth - this.Desktop.ScaleRound((float)num);
			bool flag = this.Anchor.MaxWidth != null || this.Anchor.Width != null;
			if (flag)
			{
				int num2 = this.Desktop.ScaleRound((float)((this.Anchor.MaxWidth ?? this.Anchor.Width.Value) - num));
				bool flag2;
				if (CS$<>8__locals1.maxWidth != null)
				{
					int? maxWidth = CS$<>8__locals1.maxWidth;
					int num3 = num2;
					flag2 = (maxWidth.GetValueOrDefault() > num3 & maxWidth != null);
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					CS$<>8__locals1.maxWidth = new int?(num2);
				}
			}
			bool flag4 = this.Anchor.MinWidth != null;
			if (flag4)
			{
				CS$<>8__locals1.maxWidth = new int?(Math.Max(this.Desktop.ScaleRound((float)this.Anchor.MinWidth.Value), CS$<>8__locals1.maxWidth.GetValueOrDefault()));
			}
			CS$<>8__locals1.x = 0f;
			CS$<>8__locals1.y = 0f;
			CS$<>8__locals1.longestLineWidth = 0f;
			CS$<>8__locals1.lineCount = 0;
			foreach (Label.LabelSpan labelSpan in this._textSpans)
			{
				bool flag5 = labelSpan.Text.Length == 0;
				if (!flag5)
				{
					Label.<>c__DisplayClass24_1 CS$<>8__locals2;
					CS$<>8__locals2.font = this.GetSpanFont(labelSpan);
					CS$<>8__locals2.spanText = (this.Style.RenderUppercase ? labelSpan.Text.ToUpperInvariant() : labelSpan.Text);
					CS$<>8__locals2.startIndex = 0;
					for (;;)
					{
						bool flag6 = CS$<>8__locals1.x > 0f;
						if (flag6)
						{
							CS$<>8__locals1.x += this.Desktop.Scale * this.Style.LetterSpacing;
						}
						int num4;
						float num5;
						bool flag7;
						this.<ComputeScaledMinSize>g__GrowPortion|24_1(out num4, out num5, out flag7, ref CS$<>8__locals1, ref CS$<>8__locals2);
						bool flag8 = num4 > CS$<>8__locals2.startIndex;
						if (flag8)
						{
							CS$<>8__locals1.x += num5;
						}
						bool flag9 = flag7;
						if (flag9)
						{
							int num6 = labelSpan.Text.IndexOf('\n', num4);
							bool flag10 = num6 == -1;
							if (flag10)
							{
								break;
							}
							this.<ComputeScaledMinSize>g__FinishLine|24_0(ref CS$<>8__locals1);
							CS$<>8__locals2.startIndex = num6 + 1;
						}
						else
						{
							bool flag11 = num4 < CS$<>8__locals2.spanText.Length && CS$<>8__locals2.spanText[num4] == '\n';
							if (flag11)
							{
								num4++;
							}
							CS$<>8__locals2.startIndex = num4;
							bool flag12 = num4 < CS$<>8__locals2.spanText.Length || CS$<>8__locals2.spanText[CS$<>8__locals2.spanText.Length - 1] == '\n';
							if (flag12)
							{
								this.<ComputeScaledMinSize>g__FinishLine|24_0(ref CS$<>8__locals1);
							}
							bool flag13 = num4 == CS$<>8__locals2.spanText.Length;
							if (flag13)
							{
								break;
							}
						}
					}
				}
			}
			this.<ComputeScaledMinSize>g__FinishLine|24_0(ref CS$<>8__locals1);
			Point point = base.ComputeScaledAnchorAndPaddingSize(containerMaxWidth);
			bool flag14 = this.Anchor.Width == null && this.Anchor.MaxWidth == null;
			if (flag14)
			{
				point.X = (((CS$<>8__locals1.lineCount > 1) ? containerMaxWidth : null) ?? (point.X + (int)Math.Ceiling((double)CS$<>8__locals1.longestLineWidth)));
			}
			bool flag15 = this.Anchor.MinWidth != null;
			if (flag15)
			{
				point.X = Math.Max(this.Desktop.ScaleRound((float)this.Anchor.MinWidth.Value), point.X);
			}
			bool flag16 = this.Anchor.Height == null;
			if (flag16)
			{
				point.Y += this._scaledLineHeight * CS$<>8__locals1.lineCount;
			}
			return point;
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00095314 File Offset: 0x00093514
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._fontFamily = this.Desktop.Provider.GetFontFamily(this.Style.FontName.Value);
			this._scaledLineHeight = this.Desktop.ScaleRound((float)this._fontFamily.RegularFont.Height * this.Style.FontSize / (float)this._fontFamily.RegularFont.BaseSize);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00095390 File Offset: 0x00093590
		protected override void LayoutSelf()
		{
			Label.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this._textSpanPortions.Clear();
			this._linkSpanPortions.Clear();
			this._tagSpanPortions.Clear();
			CS$<>8__locals1.maxWidth = this._rectangleAfterPadding.Width;
			CS$<>8__locals1.x = 0f;
			CS$<>8__locals1.y = 0f;
			CS$<>8__locals1.lineCount = 0;
			CS$<>8__locals1.lineTextPortions = new List<Label.LabelSpanPortion>();
			CS$<>8__locals1.lineLinkPortions = new List<Label.LabelSpanPortion>();
			CS$<>8__locals1.lineTagPortions = new List<Label.LabelSpanPortion>();
			foreach (Label.LabelSpan labelSpan in this._textSpans)
			{
				bool flag = labelSpan.Text.Length == 0;
				if (!flag)
				{
					Label.<>c__DisplayClass26_1 CS$<>8__locals2;
					CS$<>8__locals2.font = this.GetSpanFont(labelSpan);
					CS$<>8__locals2.spanText = (this.Style.RenderUppercase ? labelSpan.Text.ToUpperInvariant() : labelSpan.Text);
					CS$<>8__locals2.startIndex = 0;
					for (;;)
					{
						bool flag2 = CS$<>8__locals1.x > 0f;
						if (flag2)
						{
							CS$<>8__locals1.x += this.Desktop.Scale * this.Style.LetterSpacing;
						}
						int num;
						float num2;
						bool flag3;
						this.<LayoutSelf>g__GrowPortion|26_1(out num, out num2, out flag3, ref CS$<>8__locals1, ref CS$<>8__locals2);
						bool flag4 = num > CS$<>8__locals2.startIndex || num == 0;
						if (flag4)
						{
							string text = CS$<>8__locals2.spanText.Substring(CS$<>8__locals2.startIndex, num - CS$<>8__locals2.startIndex);
							bool flag5 = flag3;
							if (flag5)
							{
								text += "…";
							}
							Label.LabelSpanPortion item = new Label.LabelSpanPortion
							{
								Span = labelSpan,
								Text = text,
								X = CS$<>8__locals1.x,
								Y = CS$<>8__locals1.y,
								Width = num2
							};
							bool flag6 = labelSpan.Link != null;
							if (flag6)
							{
								CS$<>8__locals1.lineLinkPortions.Add(item);
							}
							else
							{
								bool flag7 = labelSpan.Params != null && labelSpan.Params.ContainsKey("tagType");
								if (flag7)
								{
									CS$<>8__locals1.lineTagPortions.Add(item);
								}
								else
								{
									CS$<>8__locals1.lineTextPortions.Add(item);
								}
							}
							CS$<>8__locals1.x += num2;
						}
						bool flag8 = flag3;
						if (flag8)
						{
							int num3 = labelSpan.Text.IndexOf('\n', num);
							bool flag9 = num3 == -1;
							if (flag9)
							{
								break;
							}
							this.<LayoutSelf>g__FinishLine|26_0(ref CS$<>8__locals1);
							CS$<>8__locals2.startIndex = num3 + 1;
						}
						else
						{
							bool flag10 = num < CS$<>8__locals2.spanText.Length && CS$<>8__locals2.spanText[num] == '\n';
							if (flag10)
							{
								num++;
							}
							CS$<>8__locals2.startIndex = num;
							bool flag11 = num < CS$<>8__locals2.spanText.Length || CS$<>8__locals2.spanText[CS$<>8__locals2.spanText.Length - 1] == '\n';
							if (flag11)
							{
								this.<LayoutSelf>g__FinishLine|26_0(ref CS$<>8__locals1);
							}
							bool flag12 = num == CS$<>8__locals2.spanText.Length;
							if (flag12)
							{
								break;
							}
						}
					}
				}
			}
			this.<LayoutSelf>g__FinishLine|26_0(ref CS$<>8__locals1);
			bool flag13 = this.Style.VerticalAlignment > LabelStyle.LabelAlignment.Start;
			if (flag13)
			{
				int num4 = this._rectangleAfterPadding.Height - this._scaledLineHeight * CS$<>8__locals1.lineCount;
				bool flag14 = this.Style.VerticalAlignment == LabelStyle.LabelAlignment.Center;
				if (flag14)
				{
					num4 /= 2;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion in this._textSpanPortions)
				{
					labelSpanPortion.Y += (float)num4;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion2 in this._linkSpanPortions)
				{
					labelSpanPortion2.Y += (float)num4;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion3 in this._tagSpanPortions)
				{
					labelSpanPortion3.Y += (float)num4;
				}
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00095854 File Offset: 0x00093A54
		protected override void PrepareForDrawSelf()
		{
			Label.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			base.PrepareForDrawSelf();
			Debug.Assert(this._textSpanPortions.Count > 0 || this._textSpans.Count == 0, "Label hasn't been laid out since text was set.", "{0}", new object[]
			{
				this
			});
			CS$<>8__locals1.underlineHeight = Math.Max(1, this.Desktop.ScaleRound(1f));
			CS$<>8__locals1.refX = this._rectangleAfterPadding.X;
			CS$<>8__locals1.refY = this._rectangleAfterPadding.Y;
			foreach (Label.LabelSpanPortion portion in this._textSpanPortions)
			{
				this.<PrepareForDrawSelf>g__DrawPortion|27_0(portion, ref CS$<>8__locals1);
			}
			foreach (Label.LabelSpanPortion portion2 in this._linkSpanPortions)
			{
				this.<PrepareForDrawSelf>g__DrawPortion|27_0(portion2, ref CS$<>8__locals1);
			}
			foreach (Label.LabelSpanPortion portion3 in this._tagSpanPortions)
			{
				this.<PrepareForDrawSelf>g__DrawPortion|27_0(portion3, ref CS$<>8__locals1);
			}
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x000959CC File Offset: 0x00093BCC
		[CompilerGenerated]
		private void <ComputeScaledMinSize>g__FinishLine|24_0(ref Label.<>c__DisplayClass24_0 A_1)
		{
			A_1.longestLineWidth = Math.Max(A_1.longestLineWidth, A_1.x);
			A_1.x = 0f;
			A_1.y += (float)this._scaledLineHeight;
			int lineCount = A_1.lineCount;
			A_1.lineCount = lineCount + 1;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00095A20 File Offset: 0x00093C20
		[CompilerGenerated]
		private void <ComputeScaledMinSize>g__GrowPortion|24_1(out int breakIndex, out float portionWidth, out bool ellipsize, ref Label.<>c__DisplayClass24_0 A_4, ref Label.<>c__DisplayClass24_1 A_5)
		{
			breakIndex = A_5.spanText.Length;
			ellipsize = false;
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			int i = A_5.startIndex;
			while (i < A_5.spanText.Length)
			{
				bool flag = A_5.spanText[i] == '\n';
				if (flag)
				{
					num -= this.Style.LetterSpacing;
					breakIndex = i;
					break;
				}
				bool flag2 = A_5.spanText[i] == ' ';
				if (flag2)
				{
					num2 = num - this.Style.LetterSpacing;
					num3 = i + 1;
				}
				float num4 = A_5.font.GetCharacterAdvance((ushort)A_5.spanText[i]) * this.Style.FontSize / (float)A_5.font.BaseSize;
				float num5 = A_4.x + this.Desktop.Scale * (num + num4);
				int? maxWidth = A_4.maxWidth;
				float? num6 = (maxWidth != null) ? new float?((float)(maxWidth.GetValueOrDefault() + 1)) : null;
				bool flag3 = (num5 > num6.GetValueOrDefault() & num6 != null) && num > 0f;
				if (flag3)
				{
					bool flag4 = !this.Style.Wrap;
					if (flag4)
					{
						float num7 = A_5.font.GetCharacterAdvance(8230) * this.Style.FontSize / (float)A_5.font.BaseSize;
						for (;;)
						{
							bool flag5;
							if (i > 0)
							{
								float num8 = A_4.x + this.Desktop.Scale * (num + num7);
								maxWidth = A_4.maxWidth;
								num6 = ((maxWidth != null) ? new float?((float)(maxWidth.GetValueOrDefault() + 1)) : null);
								flag5 = (num8 > num6.GetValueOrDefault() & num6 != null);
							}
							else
							{
								flag5 = false;
							}
							if (!flag5)
							{
								break;
							}
							i--;
							num -= A_5.font.GetCharacterAdvance((ushort)A_5.spanText[i]) * this.Style.FontSize / (float)A_5.font.BaseSize;
						}
						num += num7;
						breakIndex = i;
						ellipsize = true;
						break;
					}
					bool flag6 = num2 > 0f;
					if (flag6)
					{
						num = num2;
						breakIndex = num3;
					}
					else
					{
						breakIndex = i;
					}
					break;
				}
				else
				{
					num += num4 + ((i < A_5.spanText.Length) ? this.Style.LetterSpacing : 0f);
					i++;
				}
			}
			portionWidth = this.Desktop.Scale * num;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x00095CC4 File Offset: 0x00093EC4
		[CompilerGenerated]
		private void <LayoutSelf>g__FinishLine|26_0(ref Label.<>c__DisplayClass26_0 A_1)
		{
			bool flag = this.Style.HorizontalAlignment > LabelStyle.LabelAlignment.Start;
			if (flag)
			{
				float num = (float)A_1.maxWidth - A_1.x;
				bool flag2 = this.Style.HorizontalAlignment == LabelStyle.LabelAlignment.Center;
				if (flag2)
				{
					num /= 2f;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion in A_1.lineTextPortions)
				{
					labelSpanPortion.X += num;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion2 in A_1.lineLinkPortions)
				{
					labelSpanPortion2.X += num;
				}
				foreach (Label.LabelSpanPortion labelSpanPortion3 in A_1.lineTagPortions)
				{
					labelSpanPortion3.X += num;
				}
			}
			this._textSpanPortions.AddRange(A_1.lineTextPortions);
			A_1.lineTextPortions.Clear();
			this._linkSpanPortions.AddRange(A_1.lineLinkPortions);
			A_1.lineLinkPortions.Clear();
			this._tagSpanPortions.AddRange(A_1.lineTagPortions);
			A_1.lineTagPortions.Clear();
			A_1.x = 0f;
			A_1.y += (float)this._scaledLineHeight;
			int lineCount = A_1.lineCount;
			A_1.lineCount = lineCount + 1;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00095E8C File Offset: 0x0009408C
		[CompilerGenerated]
		private void <LayoutSelf>g__GrowPortion|26_1(out int breakIndex, out float portionWidth, out bool ellipsize, ref Label.<>c__DisplayClass26_0 A_4, ref Label.<>c__DisplayClass26_1 A_5)
		{
			breakIndex = A_5.spanText.Length;
			ellipsize = false;
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			int i = A_5.startIndex;
			while (i < A_5.spanText.Length)
			{
				bool flag = A_5.spanText[i] == '\n';
				if (flag)
				{
					num -= this.Style.LetterSpacing;
					breakIndex = i;
					break;
				}
				bool flag2 = A_5.spanText[i] == ' ';
				if (flag2)
				{
					num2 = num - this.Style.LetterSpacing;
					num3 = i + 1;
				}
				float num4 = A_5.font.GetCharacterAdvance((ushort)A_5.spanText[i]) * this.Style.FontSize / (float)A_5.font.BaseSize;
				bool flag3 = A_4.x + this.Desktop.Scale * (num + num4) > (float)(A_4.maxWidth + 1) && num > 0f;
				if (flag3)
				{
					bool flag4 = !this.Style.Wrap;
					if (flag4)
					{
						float num5 = A_5.font.GetCharacterAdvance(8230) * this.Style.FontSize / (float)A_5.font.BaseSize;
						while (i > 0 && A_4.x + this.Desktop.Scale * (num + num5) > (float)(A_4.maxWidth + 1))
						{
							i--;
							num -= A_5.font.GetCharacterAdvance((ushort)A_5.spanText[i]) * this.Style.FontSize / (float)A_5.font.BaseSize;
						}
						num += num5;
						breakIndex = i;
						ellipsize = true;
						break;
					}
					bool flag5 = num2 > 0f;
					if (flag5)
					{
						num = num2;
						breakIndex = num3;
					}
					else
					{
						breakIndex = i;
					}
					break;
				}
				else
				{
					num += num4 + ((i < A_5.spanText.Length) ? this.Style.LetterSpacing : 0f);
					i++;
				}
			}
			portionWidth = this.Desktop.Scale * num;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000960C0 File Offset: 0x000942C0
		[CompilerGenerated]
		private void <PrepareForDrawSelf>g__DrawPortion|27_0(Label.LabelSpanPortion portion, ref Label.<>c__DisplayClass27_0 A_2)
		{
			Font spanFont = this.GetSpanFont(portion.Span);
			UInt32Color color = portion.Span.Color ?? this.Style.TextColor;
			this.Desktop.Batcher2D.RequestDrawText(spanFont, this.Style.FontSize * this.Desktop.Scale, portion.Text, new Vector3((float)A_2.refX + portion.X, (float)A_2.refY + portion.Y, 0f), color, false, this.Style.RenderItalics || portion.Span.IsItalics, this.Style.LetterSpacing * this.Desktop.Scale);
			bool flag = this.Style.RenderUnderlined || portion.Span.IsUnderlined;
			if (flag)
			{
				int num = this.Desktop.ScaleRound((float)spanFont.Height * this.Style.FontSize / (float)spanFont.BaseSize);
				this.Desktop.Batcher2D.RequestDrawTexture(this.Desktop.Provider.WhitePixel.Texture, this.Desktop.Provider.WhitePixel.Rectangle, new Rectangle((int)((float)A_2.refX + portion.X), (int)((float)A_2.refY + portion.Y + (float)num), (int)portion.Width, A_2.underlineHeight), color);
			}
		}

		// Token: 0x04001BFD RID: 7165
		public Action<string> LinkActivating;

		// Token: 0x04001BFE RID: 7166
		public Action<Label.LabelSpanPortion> TagMouseEntered;

		// Token: 0x04001BFF RID: 7167
		private Label.LabelSpanPortion _currentlyHoveredTag;

		// Token: 0x04001C00 RID: 7168
		protected readonly List<Label.LabelSpan> _textSpans = new List<Label.LabelSpan>();

		// Token: 0x04001C01 RID: 7169
		protected readonly List<Label.LabelSpanPortion> _textSpanPortions = new List<Label.LabelSpanPortion>();

		// Token: 0x04001C02 RID: 7170
		protected readonly List<Label.LabelSpanPortion> _linkSpanPortions = new List<Label.LabelSpanPortion>();

		// Token: 0x04001C03 RID: 7171
		protected readonly List<Label.LabelSpanPortion> _tagSpanPortions = new List<Label.LabelSpanPortion>();

		// Token: 0x04001C04 RID: 7172
		[UIMarkupProperty]
		public LabelStyle Style = new LabelStyle();

		// Token: 0x04001C05 RID: 7173
		private FontFamily _fontFamily;

		// Token: 0x04001C06 RID: 7174
		private int _scaledLineHeight;

		// Token: 0x02000D30 RID: 3376
		[UIMarkupData]
		public class LabelSpan
		{
			// Token: 0x0400410B RID: 16651
			public string Text;

			// Token: 0x0400410C RID: 16652
			public bool IsUppercase;

			// Token: 0x0400410D RID: 16653
			public bool IsBold;

			// Token: 0x0400410E RID: 16654
			public bool IsItalics;

			// Token: 0x0400410F RID: 16655
			public bool IsUnderlined;

			// Token: 0x04004110 RID: 16656
			public UInt32Color? Color;

			// Token: 0x04004111 RID: 16657
			public string Link;

			// Token: 0x04004112 RID: 16658
			public Dictionary<string, object> Params;
		}

		// Token: 0x02000D31 RID: 3377
		public class LabelSpanPortion
		{
			// Token: 0x04004113 RID: 16659
			public Label.LabelSpan Span;

			// Token: 0x04004114 RID: 16660
			public string Text;

			// Token: 0x04004115 RID: 16661
			public float X;

			// Token: 0x04004116 RID: 16662
			public float Y;

			// Token: 0x04004117 RID: 16663
			public float Width;

			// Token: 0x04004118 RID: 16664
			public Point CenterPoint;
		}
	}
}
