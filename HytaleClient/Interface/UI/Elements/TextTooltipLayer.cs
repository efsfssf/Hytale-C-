using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000879 RID: 2169
	internal class TextTooltipLayer : BaseTooltipLayer
	{
		// Token: 0x17001086 RID: 4230
		// (set) Token: 0x06003D46 RID: 15686 RVA: 0x0009C196 File Offset: 0x0009A396
		[UIMarkupProperty]
		public string Text
		{
			set
			{
				this._label.Text = value;
			}
		}

		// Token: 0x17001087 RID: 4231
		// (set) Token: 0x06003D47 RID: 15687 RVA: 0x0009C1A5 File Offset: 0x0009A3A5
		[UIMarkupProperty]
		public IList<Label.LabelSpan> TextSpans
		{
			set
			{
				this._label.TextSpans = value;
			}
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x0009C1B4 File Offset: 0x0009A3B4
		public TextTooltipLayer(Desktop desktop) : base(desktop)
		{
			this._layoutMode = LayoutMode.Top;
			Group group = new Group(this.Desktop, this);
			group.LayoutMode = LayoutMode.Left;
			group.Anchor.Left = new int?(0);
			this._wrapper = group;
			this._group = new Group(this.Desktop, this._wrapper);
			this._label = new Label(this.Desktop, this._group);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0009C238 File Offset: 0x0009A438
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._label.Style = this.Style.LabelStyle;
			this._group.Background = this.Style.Background;
			this._group.Padding = this.Style.Padding;
			this._wrapper.Anchor.Width = this.Style.MaxWidth;
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x0009C2AC File Offset: 0x0009A4AC
		protected override void UpdatePosition()
		{
			this.Anchor.Left = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.X));
			this.Anchor.Top = new int?(this.Desktop.UnscaleRound((float)this.Desktop.MousePosition.Y) + 30);
			Point point = this._group.ComputeScaledMinSize((this.Style.MaxWidth != null) ? new int?(this.Desktop.ScaleRound((float)this.Style.MaxWidth.Value)) : null, null);
			bool flag;
			if (this.Desktop.MousePosition.X + point.X <= this.Desktop.RootLayoutRectangle.Width && this.Style.Alignment.GetValueOrDefault() != TextTooltipStyle.TooltipAlignment.BottomLeft)
			{
				TextTooltipStyle.TooltipAlignment? alignment = this.Style.Alignment;
				TextTooltipStyle.TooltipAlignment tooltipAlignment = TextTooltipStyle.TooltipAlignment.TopLeft;
				flag = (alignment.GetValueOrDefault() == tooltipAlignment & alignment != null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.Anchor.Left = this.Anchor.Left - this.Desktop.UnscaleRound((float)point.X);
			}
			bool flag3;
			if (this.Desktop.MousePosition.Y + point.Y + this.Desktop.ScaleRound(30f) <= this.Desktop.RootLayoutRectangle.Height)
			{
				TextTooltipStyle.TooltipAlignment? alignment = this.Style.Alignment;
				TextTooltipStyle.TooltipAlignment tooltipAlignment = TextTooltipStyle.TooltipAlignment.TopLeft;
				if (!(alignment.GetValueOrDefault() == tooltipAlignment & alignment != null))
				{
					flag3 = (this.Style.Alignment.GetValueOrDefault() == TextTooltipStyle.TooltipAlignment.TopRight);
					goto IL_1D7;
				}
			}
			flag3 = true;
			IL_1D7:
			bool flag4 = flag3;
			if (flag4)
			{
				this.Anchor.Top = this.Anchor.Top - (this.Desktop.UnscaleRound((float)point.Y) + 60);
			}
		}

		// Token: 0x04001C84 RID: 7300
		[UIMarkupProperty]
		public TextTooltipStyle Style = new TextTooltipStyle();

		// Token: 0x04001C85 RID: 7301
		private readonly Group _wrapper;

		// Token: 0x04001C86 RID: 7302
		private readonly Group _group;

		// Token: 0x04001C87 RID: 7303
		private readonly Label _label;
	}
}
