using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000838 RID: 2104
	[UIMarkupData]
	public class InputFieldStyle
	{
		// Token: 0x04001AC8 RID: 6856
		public UIFontName FontName = new UIFontName("Default");

		// Token: 0x04001AC9 RID: 6857
		public float FontSize = 16f;

		// Token: 0x04001ACA RID: 6858
		public UInt32Color TextColor = UInt32Color.White;

		// Token: 0x04001ACB RID: 6859
		public bool RenderUppercase;

		// Token: 0x04001ACC RID: 6860
		public bool RenderBold;

		// Token: 0x04001ACD RID: 6861
		public bool RenderItalics;
	}
}
