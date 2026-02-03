using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083A RID: 2106
	[UIMarkupData]
	public class LabelStyle
	{
		// Token: 0x17001029 RID: 4137
		// (set) Token: 0x06003A9F RID: 15007 RVA: 0x00085B8C File Offset: 0x00083D8C
		public LabelStyle.LabelAlignment Alignment
		{
			set
			{
				this.VerticalAlignment = value;
				this.HorizontalAlignment = value;
			}
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00085BAC File Offset: 0x00083DAC
		public LabelStyle Clone()
		{
			return new LabelStyle
			{
				HorizontalAlignment = this.HorizontalAlignment,
				VerticalAlignment = this.VerticalAlignment,
				Wrap = this.Wrap,
				FontName = this.FontName,
				FontSize = this.FontSize,
				TextColor = this.TextColor,
				LetterSpacing = this.LetterSpacing,
				RenderBold = this.RenderBold,
				RenderItalics = this.RenderItalics,
				RenderUnderlined = this.RenderUnderlined,
				RenderUppercase = this.RenderUppercase
			};
		}

		// Token: 0x04001AD3 RID: 6867
		public LabelStyle.LabelAlignment HorizontalAlignment = LabelStyle.LabelAlignment.Start;

		// Token: 0x04001AD4 RID: 6868
		public LabelStyle.LabelAlignment VerticalAlignment = LabelStyle.LabelAlignment.Start;

		// Token: 0x04001AD5 RID: 6869
		public bool Wrap = false;

		// Token: 0x04001AD6 RID: 6870
		public UIFontName FontName = new UIFontName("Default");

		// Token: 0x04001AD7 RID: 6871
		public float FontSize = 16f;

		// Token: 0x04001AD8 RID: 6872
		public UInt32Color TextColor = UInt32Color.White;

		// Token: 0x04001AD9 RID: 6873
		public float LetterSpacing = 0f;

		// Token: 0x04001ADA RID: 6874
		public bool RenderUppercase;

		// Token: 0x04001ADB RID: 6875
		public bool RenderBold;

		// Token: 0x04001ADC RID: 6876
		public bool RenderItalics;

		// Token: 0x04001ADD RID: 6877
		public bool RenderUnderlined;

		// Token: 0x02000D13 RID: 3347
		public enum LabelAlignment
		{
			// Token: 0x040040B0 RID: 16560
			Start,
			// Token: 0x040040B1 RID: 16561
			Center,
			// Token: 0x040040B2 RID: 16562
			End
		}
	}
}
