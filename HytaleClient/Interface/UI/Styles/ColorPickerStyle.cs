using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000831 RID: 2097
	[UIMarkupData]
	public class ColorPickerStyle
	{
		// Token: 0x04001A7B RID: 6779
		public PatchStyle ButtonBackground;

		// Token: 0x04001A7C RID: 6780
		public PatchStyle ButtonFill;

		// Token: 0x04001A7D RID: 6781
		public PatchStyle OpacitySelectorBackground;

		// Token: 0x04001A7E RID: 6782
		public InputFieldDecorationStyle TextFieldDecoration;

		// Token: 0x04001A7F RID: 6783
		public InputFieldStyle TextFieldInputStyle = new InputFieldStyle();

		// Token: 0x04001A80 RID: 6784
		public Padding TextFieldPadding;

		// Token: 0x04001A81 RID: 6785
		public int TextFieldHeight = 28;
	}
}
