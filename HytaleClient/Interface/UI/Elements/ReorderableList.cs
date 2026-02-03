using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000871 RID: 2161
	[UIMarkupElement(AcceptsChildren = true)]
	public class ReorderableList : Element
	{
		// Token: 0x17001070 RID: 4208
		// (set) Token: 0x06003CEB RID: 15595 RVA: 0x00099DFA File Offset: 0x00097FFA
		[UIMarkupProperty]
		public ScrollbarStyle ScrollbarStyle
		{
			set
			{
				this._scrollbarStyle = value;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00099E03 File Offset: 0x00098003
		// (set) Token: 0x06003CED RID: 15597 RVA: 0x00099E0B File Offset: 0x0009800B
		[UIMarkupProperty]
		public new LayoutMode LayoutMode
		{
			get
			{
				return this._layoutMode;
			}
			set
			{
				this._layoutMode = value;
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00099E15 File Offset: 0x00098015
		public ReorderableList(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._layoutMode = LayoutMode.Top;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00099E2F File Offset: 0x0009802F
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._dropIndicatorTexturePatch = ((this.DropIndicatorBackground != null) ? this.Desktop.MakeTexturePatch(this.DropIndicatorBackground) : null);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00099E5C File Offset: 0x0009805C
		protected override void LayoutSelf()
		{
			bool flag = this._dropTargetIndex == -1;
			if (!flag)
			{
				bool flag2 = this._layoutMode == LayoutMode.Top || this._layoutMode == LayoutMode.Bottom || this._layoutMode == LayoutMode.Middle || this._layoutMode == LayoutMode.MiddleCenter || this._layoutMode == LayoutMode.TopScrolling || this._layoutMode == LayoutMode.BottomScrolling;
				if (flag2)
				{
					bool flag3 = this._dropTargetIndex == base.Children.Count;
					int num;
					Rectangle anchoredRectangle;
					if (flag3)
					{
						Element element = base.Children[base.Children.Count - 1];
						num = element.AnchoredRectangle.Bottom;
						anchoredRectangle = element.AnchoredRectangle;
					}
					else
					{
						Element element2 = base.Children[this._dropTargetIndex];
						num = element2.AnchoredRectangle.Top;
						anchoredRectangle = element2.AnchoredRectangle;
					}
					bool flag4 = this._dropTargetIndex > 0 && this._dropTargetIndex < base.Children.Count;
					if (flag4)
					{
						Element element3 = base.Children[this._dropTargetIndex - 1];
						float num2 = (float)(anchoredRectangle.Top - element3.AnchoredRectangle.Bottom) / 2f;
						num -= (int)num2;
					}
					bool flag5 = this._dropTargetIndex == base.Children.Count;
					if (flag5)
					{
						num -= this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Height.GetValueOrDefault());
					}
					else
					{
						bool flag6 = this._dropTargetIndex != 0;
						if (flag6)
						{
							num -= this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Height.GetValueOrDefault() / 2f);
						}
					}
					float num3 = (float)anchoredRectangle.Left + (float)this.DropIndicatorAnchor.Left.GetValueOrDefault() * this.Desktop.Scale;
					float num4 = (float)anchoredRectangle.Right - (float)this.DropIndicatorAnchor.Right.GetValueOrDefault() * this.Desktop.Scale;
					bool flag7 = this.DropIndicatorAnchor.Width != null;
					if (flag7)
					{
						float num5 = (float)this.DropIndicatorAnchor.Width.Value * this.Desktop.Scale;
						bool flag8 = this.DropIndicatorAnchor.Left != null;
						if (flag8)
						{
							bool flag9 = this.DropIndicatorAnchor.Right == null;
							if (flag9)
							{
								num4 = num3 + num5;
							}
						}
						else
						{
							bool flag10 = this.DropIndicatorAnchor.Right != null;
							if (flag10)
							{
								num3 = num4 - num5;
							}
							else
							{
								num3 = (float)anchoredRectangle.Center.X - num5 / 2f;
								num4 = num3 + num5;
							}
						}
					}
					this._dropIndicatorRectangle = new Rectangle(MathHelper.Round(num3), num, MathHelper.Round(num4 - num3), this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Height.GetValueOrDefault()));
				}
				else
				{
					bool flag11 = this._dropTargetIndex == base.Children.Count;
					int num6;
					Rectangle anchoredRectangle2;
					if (flag11)
					{
						Element element4 = base.Children[base.Children.Count - 1];
						num6 = element4.AnchoredRectangle.Right;
						anchoredRectangle2 = element4.AnchoredRectangle;
					}
					else
					{
						Element element5 = base.Children[this._dropTargetIndex];
						num6 = element5.AnchoredRectangle.Left;
						anchoredRectangle2 = element5.AnchoredRectangle;
					}
					bool flag12 = this._dropTargetIndex > 0 && this._dropTargetIndex < base.Children.Count;
					if (flag12)
					{
						Element element6 = base.Children[this._dropTargetIndex - 1];
						float num7 = (float)(anchoredRectangle2.Left - element6.AnchoredRectangle.Right) / 2f;
						num6 -= (int)num7;
					}
					bool flag13 = this._dropTargetIndex == base.Children.Count;
					if (flag13)
					{
						num6 -= this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Width.GetValueOrDefault());
					}
					else
					{
						bool flag14 = this._dropTargetIndex != 0;
						if (flag14)
						{
							num6 -= this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Width.GetValueOrDefault() / 2f);
						}
					}
					float num8 = (float)anchoredRectangle2.Top + (float)this.DropIndicatorAnchor.Top.GetValueOrDefault() * this.Desktop.Scale;
					float num9 = (float)anchoredRectangle2.Bottom - (float)this.DropIndicatorAnchor.Bottom.GetValueOrDefault() * this.Desktop.Scale;
					bool flag15 = this.DropIndicatorAnchor.Height != null;
					if (flag15)
					{
						float num10 = (float)this.DropIndicatorAnchor.Height.Value * this.Desktop.Scale;
						bool flag16 = this.DropIndicatorAnchor.Top != null;
						if (flag16)
						{
							bool flag17 = this.DropIndicatorAnchor.Right == null;
							if (flag17)
							{
								num9 = num8 + num10;
							}
						}
						else
						{
							bool flag18 = this.DropIndicatorAnchor.Bottom != null;
							if (flag18)
							{
								num8 = num9 - num10;
							}
							else
							{
								num8 = (float)anchoredRectangle2.Center.X - num10 / 2f;
								num9 = num8 + num10;
							}
						}
					}
					this._dropIndicatorRectangle = new Rectangle(num6, MathHelper.Round(num8), this.Desktop.ScaleRound((float)this.DropIndicatorAnchor.Width.GetValueOrDefault()), MathHelper.Round(num9 - num8));
				}
			}
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0009A40B File Offset: 0x0009860B
		public void SetDropTargetIndex(int index)
		{
			this._dropTargetIndex = index;
			this.LayoutSelf();
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0009A41C File Offset: 0x0009861C
		protected override void PrepareForDrawContent()
		{
			base.PrepareForDrawContent();
			bool flag = this._dropTargetIndex == -1;
			if (!flag)
			{
				bool flag2 = this._dropIndicatorTexturePatch != null;
				if (flag2)
				{
					this.Desktop.Graphics.Batcher2D.RequestDrawPatch(this._dropIndicatorTexturePatch, this._dropIndicatorRectangle, this.Desktop.Scale);
				}
			}
		}

		// Token: 0x04001C55 RID: 7253
		public Action<int, int> ElementReordered;

		// Token: 0x04001C56 RID: 7254
		private int _dropTargetIndex = -1;

		// Token: 0x04001C57 RID: 7255
		[UIMarkupProperty]
		public Anchor DropIndicatorAnchor;

		// Token: 0x04001C58 RID: 7256
		[UIMarkupProperty]
		public PatchStyle DropIndicatorBackground;

		// Token: 0x04001C59 RID: 7257
		private TexturePatch _dropIndicatorTexturePatch;

		// Token: 0x04001C5A RID: 7258
		private Rectangle _dropIndicatorRectangle;
	}
}
