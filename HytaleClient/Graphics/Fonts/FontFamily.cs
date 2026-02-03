using System;

namespace HytaleClient.Graphics.Fonts
{
	// Token: 0x02000ABF RID: 2751
	public class FontFamily
	{
		// Token: 0x060056C6 RID: 22214 RVA: 0x001A1672 File Offset: 0x0019F872
		public FontFamily(Font regularFont, Font boldFont)
		{
			this.RegularFont = regularFont;
			this.BoldFont = boldFont;
		}

		// Token: 0x04003440 RID: 13376
		public readonly Font RegularFont;

		// Token: 0x04003441 RID: 13377
		public readonly Font BoldFont;
	}
}
