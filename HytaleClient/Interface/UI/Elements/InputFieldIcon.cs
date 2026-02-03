using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200087B RID: 2171
	[UIMarkupData]
	public class InputFieldIcon
	{
		// Token: 0x04001C8F RID: 7311
		public PatchStyle Texture;

		// Token: 0x04001C90 RID: 7312
		public int Width;

		// Token: 0x04001C91 RID: 7313
		public int Height;

		// Token: 0x04001C92 RID: 7314
		public int Offset;

		// Token: 0x04001C93 RID: 7315
		public InputFieldIcon.InputFieldIconSide Side;

		// Token: 0x02000D53 RID: 3411
		public enum InputFieldIconSide
		{
			// Token: 0x0400418E RID: 16782
			Left,
			// Token: 0x0400418F RID: 16783
			Right
		}
	}
}
