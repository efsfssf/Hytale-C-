using System;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BA9 RID: 2985
	[UIMarkupElement(AcceptsChildren = true)]
	public class DynamicPane : Group
	{
		// Token: 0x06005C96 RID: 23702 RVA: 0x001D3786 File Offset: 0x001D1986
		public DynamicPane(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x001D37A8 File Offset: 0x001D19A8
		protected override void OnMounted()
		{
			Debug.Assert(this.Parent is DynamicPaneContainer);
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x001D37BF File Offset: 0x001D19BF
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._resizerPatch = ((this.ResizerBackground != null && this.ResizeAt != DynamicPane.ResizeType.None) ? this.Desktop.MakeTexturePatch(this.ResizerBackground) : null);
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x001D37F4 File Offset: 0x001D19F4
		private DynamicPane.LayoutDirection GetParentLayoutDirection()
		{
			switch (this.Parent.LayoutMode)
			{
			case LayoutMode.Left:
			case LayoutMode.Right:
				return DynamicPane.LayoutDirection.Horizontal;
			case LayoutMode.Top:
			case LayoutMode.Bottom:
				return DynamicPane.LayoutDirection.Vertical;
			}
			return DynamicPane.LayoutDirection.Invalid;
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x001D3840 File Offset: 0x001D1A40
		protected override void LayoutSelf()
		{
			DynamicPane.LayoutDirection parentLayoutDirection = this.GetParentLayoutDirection();
			bool flag = parentLayoutDirection == DynamicPane.LayoutDirection.Invalid;
			if (flag)
			{
				this._resizerRectangle = Rectangle.Empty;
			}
			else
			{
				int num = this.Desktop.ScaleRound((float)this.ResizerSize);
				DynamicPane.ResizeType resizeAt = this.ResizeAt;
				DynamicPane.ResizeType resizeType = resizeAt;
				if (resizeType != DynamicPane.ResizeType.Start)
				{
					if (resizeType != DynamicPane.ResizeType.End)
					{
						this._resizerRectangle = Rectangle.Empty;
					}
					else
					{
						bool flag2 = parentLayoutDirection == DynamicPane.LayoutDirection.Vertical;
						if (flag2)
						{
							this._resizerRectangle = new Rectangle(this._anchoredRectangle.X, this._anchoredRectangle.Bottom - num, this._anchoredRectangle.Width, num);
							this._rectangleAfterPadding.Height = this._rectangleAfterPadding.Height - num;
							this._backgroundRectangle.Height = this._backgroundRectangle.Height - num;
						}
						else
						{
							this._resizerRectangle = new Rectangle(this._anchoredRectangle.Right - num, this._anchoredRectangle.Y, num, this._anchoredRectangle.Height);
							this._rectangleAfterPadding.Width = this._rectangleAfterPadding.Width - num;
							this._backgroundRectangle.Width = this._backgroundRectangle.Width - num;
						}
					}
				}
				else
				{
					bool flag3 = parentLayoutDirection == DynamicPane.LayoutDirection.Vertical;
					if (flag3)
					{
						this._resizerRectangle = new Rectangle(this._anchoredRectangle.X, this._anchoredRectangle.Y, this._anchoredRectangle.Width, num);
						this._rectangleAfterPadding.Y = this._rectangleAfterPadding.Y + num;
						this._rectangleAfterPadding.Height = this._rectangleAfterPadding.Height - num;
						this._backgroundRectangle.Y = this._backgroundRectangle.Y + num;
						this._backgroundRectangle.Height = this._backgroundRectangle.Height - num;
					}
					else
					{
						this._resizerRectangle = new Rectangle(this._anchoredRectangle.X, this._anchoredRectangle.Y, num, this._anchoredRectangle.Height);
						this._rectangleAfterPadding.X = this._rectangleAfterPadding.X + num;
						this._rectangleAfterPadding.Width = this._rectangleAfterPadding.Width - num;
						this._backgroundRectangle.X = this._backgroundRectangle.X + num;
						this._backgroundRectangle.Width = this._backgroundRectangle.Width - num;
					}
				}
			}
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x001D3A50 File Offset: 0x001D1C50
		public override Element HitTest(Point position)
		{
			return (this.ResizeAt != DynamicPane.ResizeType.None && this._resizerRectangle.Contains(position)) ? this : base.HitTest(position);
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x001D3A82 File Offset: 0x001D1C82
		public override void OnMouseOut()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			this._isResizerHovered = false;
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x001D3AA2 File Offset: 0x001D1CA2
		public override void OnMouseIn()
		{
			this.UpdateResizerState();
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x001D3AAC File Offset: 0x001D1CAC
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			Action mouseButtonReleased = this.MouseButtonReleased;
			if (mouseButtonReleased != null)
			{
				mouseButtonReleased();
			}
			this.Desktop.RefreshHover();
			this.UpdateResizerState();
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x001D3AD4 File Offset: 0x001D1CD4
		protected override void OnMouseMove()
		{
			this.UpdateResizerState();
			bool flag = base.CapturedMouseButton == null;
			if (!flag)
			{
				DynamicPane.LayoutDirection parentLayoutDirection = this.GetParentLayoutDirection();
				bool flag2 = parentLayoutDirection == DynamicPane.LayoutDirection.Invalid;
				if (!flag2)
				{
					bool flag3 = parentLayoutDirection == DynamicPane.LayoutDirection.Vertical;
					if (flag3)
					{
						int maxHeight = this.GetMaxHeight();
						int num = (this.ResizeAt == DynamicPane.ResizeType.End) ? this.Desktop.UnscaleRound((float)(this.Desktop.MousePosition.Y - base.AnchoredRectangle.Y)) : this.Desktop.UnscaleRound((float)(base.AnchoredRectangle.Bottom - this.Desktop.MousePosition.Y));
						bool flag4 = num < this.MinSize;
						if (flag4)
						{
							num = this.MinSize;
						}
						else
						{
							bool flag5 = num > maxHeight;
							if (flag5)
							{
								num = maxHeight;
							}
						}
						this.Anchor.Height = new int?(num);
					}
					else
					{
						int maxWidth = this.GetMaxWidth();
						int num2 = (this.ResizeAt == DynamicPane.ResizeType.End) ? this.Desktop.UnscaleRound((float)(this.Desktop.MousePosition.X - base.AnchoredRectangle.X)) : this.Desktop.UnscaleRound((float)(base.AnchoredRectangle.Right - this.Desktop.MousePosition.X));
						bool flag6 = num2 < this.MinSize;
						if (flag6)
						{
							num2 = this.MinSize;
						}
						else
						{
							bool flag7 = num2 > maxWidth;
							if (flag7)
							{
								num2 = maxWidth;
							}
						}
						this.Anchor.Width = new int?(num2);
					}
					this.Parent.Layout(null, true);
				}
			}
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x001D3C90 File Offset: 0x001D1E90
		private int GetMaxWidth()
		{
			int num = this.Desktop.UnscaleRound((float)this.Parent.AnchoredRectangle.Width);
			foreach (Element element in this.Parent.Children)
			{
				bool flag = element == this || !element.IsMounted;
				if (!flag)
				{
					DynamicPane dynamicPane = element as DynamicPane;
					bool flag2 = dynamicPane != null;
					if (flag2)
					{
						bool flag3 = element.FlexWeight > 0;
						if (flag3)
						{
							num -= dynamicPane.MinSize;
						}
						else
						{
							bool flag4 = element.Anchor.Width != null;
							if (flag4)
							{
								num -= element.Anchor.Width.Value;
							}
						}
					}
					else
					{
						bool flag5 = element.Anchor.Width != null;
						if (flag5)
						{
							num -= element.Anchor.Width.Value;
						}
						else
						{
							bool flag6 = element.FlexWeight > 0;
							if (flag6)
							{
								num -= 50;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x001D3DD8 File Offset: 0x001D1FD8
		private int GetMaxHeight()
		{
			int num = this.Desktop.UnscaleRound((float)this.Parent.AnchoredRectangle.Height);
			foreach (Element element in this.Parent.Children)
			{
				bool flag = element == this || !element.IsMounted;
				if (!flag)
				{
					DynamicPane dynamicPane = element as DynamicPane;
					bool flag2 = dynamicPane != null;
					if (flag2)
					{
						bool flag3 = element.FlexWeight > 0;
						if (flag3)
						{
							num -= dynamicPane.MinSize;
						}
						else
						{
							bool flag4 = element.Anchor.Height != null;
							if (flag4)
							{
								num -= element.Anchor.Height.Value;
							}
						}
					}
					else
					{
						bool flag5 = element.Anchor.Height != null;
						if (flag5)
						{
							num -= element.Anchor.Height.Value;
						}
						else
						{
							bool flag6 = element.FlexWeight > 0;
							if (flag6)
							{
								num -= 50;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06005CA2 RID: 23714 RVA: 0x001D3F10 File Offset: 0x001D2110
		private void UpdateResizerState()
		{
			bool isResizerHovered = this._isResizerHovered;
			if (isResizerHovered)
			{
				bool flag = !this._resizerRectangle.Contains(this.Desktop.MousePosition) && base.CapturedMouseButton == null;
				if (flag)
				{
					this._isResizerHovered = false;
					SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
				}
			}
			else
			{
				bool flag2 = this._resizerRectangle.Contains(this.Desktop.MousePosition) || base.CapturedMouseButton != null;
				if (flag2)
				{
					this._isResizerHovered = true;
					SDL.SDL_SetCursor((this.GetParentLayoutDirection() == DynamicPane.LayoutDirection.Vertical) ? this.Desktop.Cursors.SizeNS : this.Desktop.Cursors.SizeWE);
				}
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x001D3FE4 File Offset: 0x001D21E4
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this.ResizerBackground != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._resizerPatch, this._resizerRectangle, this.Desktop.Scale);
			}
		}

		// Token: 0x04003A01 RID: 14849
		public Action MouseButtonReleased;

		// Token: 0x04003A02 RID: 14850
		[UIMarkupProperty]
		public int MinSize = 50;

		// Token: 0x04003A03 RID: 14851
		[UIMarkupProperty]
		public DynamicPane.ResizeType ResizeAt = DynamicPane.ResizeType.None;

		// Token: 0x04003A04 RID: 14852
		[UIMarkupProperty]
		public int ResizerSize = 1;

		// Token: 0x04003A05 RID: 14853
		[UIMarkupProperty]
		public PatchStyle ResizerBackground;

		// Token: 0x04003A06 RID: 14854
		private TexturePatch _resizerPatch;

		// Token: 0x04003A07 RID: 14855
		private Rectangle _resizerRectangle;

		// Token: 0x04003A08 RID: 14856
		private bool _isResizerHovered;

		// Token: 0x02000FAF RID: 4015
		public enum ResizeType
		{
			// Token: 0x04004BB6 RID: 19382
			None,
			// Token: 0x04004BB7 RID: 19383
			Start,
			// Token: 0x04004BB8 RID: 19384
			End
		}

		// Token: 0x02000FB0 RID: 4016
		private enum LayoutDirection
		{
			// Token: 0x04004BBA RID: 19386
			Invalid,
			// Token: 0x04004BBB RID: 19387
			Vertical,
			// Token: 0x04004BBC RID: 19388
			Horizontal
		}
	}
}
