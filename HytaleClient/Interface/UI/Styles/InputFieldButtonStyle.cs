using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000835 RID: 2101
	[UIMarkupData]
	public class InputFieldButtonStyle
	{
		// Token: 0x04001ABA RID: 6842
		public PatchStyle Texture;

		// Token: 0x04001ABB RID: 6843
		public PatchStyle HoveredTexture;

		// Token: 0x04001ABC RID: 6844
		public PatchStyle PressedTexture;

		// Token: 0x04001ABD RID: 6845
		public int Width;

		// Token: 0x04001ABE RID: 6846
		public int Height;

		// Token: 0x04001ABF RID: 6847
		public int Offset;

		// Token: 0x04001AC0 RID: 6848
		public InputFieldButtonStyle.InputFieldButtonSide Side = InputFieldButtonStyle.InputFieldButtonSide.Right;

		// Token: 0x02000D12 RID: 3346
		public enum InputFieldButtonSide
		{
			// Token: 0x040040AD RID: 16557
			Left,
			// Token: 0x040040AE RID: 16558
			Right
		}
	}
}
