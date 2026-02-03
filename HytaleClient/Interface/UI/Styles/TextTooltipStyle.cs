using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000840 RID: 2112
	[UIMarkupData]
	public class TextTooltipStyle
	{
		// Token: 0x04001B05 RID: 6917
		public LabelStyle LabelStyle = new LabelStyle();

		// Token: 0x04001B06 RID: 6918
		public Padding Padding = new Padding
		{
			Full = new int?(10)
		};

		// Token: 0x04001B07 RID: 6919
		public PatchStyle Background = new PatchStyle(221U);

		// Token: 0x04001B08 RID: 6920
		public int? MaxWidth;

		// Token: 0x04001B09 RID: 6921
		public TextTooltipStyle.TooltipAlignment? Alignment;

		// Token: 0x02000D14 RID: 3348
		public enum TooltipAlignment
		{
			// Token: 0x040040B4 RID: 16564
			TopLeft,
			// Token: 0x040040B5 RID: 16565
			TopRight,
			// Token: 0x040040B6 RID: 16566
			BottomLeft,
			// Token: 0x040040B7 RID: 16567
			BottomRight
		}
	}
}
