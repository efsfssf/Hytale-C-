using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface
{
	// Token: 0x02000805 RID: 2053
	public class MarkupErrorOverlay : Element
	{
		// Token: 0x06003901 RID: 14593 RVA: 0x00076957 File Offset: 0x00074B57
		public MarkupErrorOverlay(Desktop desktop, Element parent, string title) : base(desktop, parent)
		{
			this._title = title;
			this._layoutMode = LayoutMode.Top;
			this.Background = new PatchStyle(170U);
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x00076984 File Offset: 0x00074B84
		public void Setup(string message, TextParserSpan span)
		{
			base.Clear();
			LabelStyle labelStyle = new LabelStyle();
			labelStyle.FontSize = 20f;
			labelStyle.TextColor = UInt32Color.FromRGBA(3722305023U);
			LabelStyle style = new LabelStyle
			{
				FontSize = 20f
			};
			LabelStyle labelStyle2 = new LabelStyle();
			labelStyle2.FontSize = 20f;
			labelStyle2.FontName = new UIFontName("Mono");
			labelStyle2.TextColor = UInt32Color.FromRGBA(2863311615U);
			labelStyle2.Wrap = true;
			labelStyle2.HorizontalAlignment = LabelStyle.LabelAlignment.End;
			LabelStyle style2 = new LabelStyle
			{
				FontSize = 20f,
				FontName = new UIFontName("Mono"),
				Wrap = true
			};
			int num;
			int num2;
			string text;
			string text2;
			string text3;
			span.GetContext(3, out num, out num2, out text, out text2, out text3);
			Label label = new Label(this.Desktop, this);
			label.Anchor = new Anchor
			{
				Full = new int?(10)
			};
			label.Style = new LabelStyle
			{
				FontSize = 30f,
				RenderBold = true
			};
			label.Text = this._title;
			Label label2 = new Label(this.Desktop, this);
			label2.Style = style;
			label2.Anchor = new Anchor
			{
				Full = new int?(10)
			};
			label2.TextSpans = new List<Label.LabelSpan>
			{
				new Label.LabelSpan
				{
					Text = string.Format("{0} ({1}:{2})", span.Parser.SourcePath, num + 1, num2 + 1),
					Color = new UInt32Color?(UInt32Color.FromRGBA(3435973887U))
				},
				new Label.LabelSpan
				{
					Text = " — "
				},
				new Label.LabelSpan
				{
					Text = message,
					IsBold = true
				}
			};
			Group parent = new Group(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Full = new int?(10)
				},
				Background = new PatchStyle(170U),
				LayoutMode = LayoutMode.Top
			};
			Label label3 = new Label(this.Desktop, parent)
			{
				Style = style2,
				Anchor = new Anchor
				{
					Full = new int?(20)
				}
			};
			bool flag = text2.Length == 0;
			if (flag)
			{
				bool flag2 = text3.Length > 0;
				if (flag2)
				{
					text2 = text3.Substring(0, 1);
					text3 = text3.Substring(1);
				}
				else
				{
					text2 = " ";
				}
			}
			label3.TextSpans = new List<Label.LabelSpan>
			{
				new Label.LabelSpan
				{
					Text = text
				},
				new Label.LabelSpan
				{
					Text = text2,
					IsUnderlined = true,
					Color = new UInt32Color?(UInt32Color.FromRGBA(4282664191U))
				},
				new Label.LabelSpan
				{
					Text = text3
				}
			};
		}

		// Token: 0x040018BE RID: 6334
		private readonly string _title;
	}
}
