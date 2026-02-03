using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000834 RID: 2100
	[UIMarkupData]
	internal class FileDropdownBoxStyle
	{
		// Token: 0x04001AAD RID: 6829
		public PatchStyle DefaultBackground;

		// Token: 0x04001AAE RID: 6830
		public PatchStyle HoveredBackground;

		// Token: 0x04001AAF RID: 6831
		public PatchStyle PressedBackground;

		// Token: 0x04001AB0 RID: 6832
		public UIPath DefaultArrowTexturePath;

		// Token: 0x04001AB1 RID: 6833
		public UIPath HoveredArrowTexturePath;

		// Token: 0x04001AB2 RID: 6834
		public UIPath PressedArrowTexturePath;

		// Token: 0x04001AB3 RID: 6835
		public int ArrowWidth;

		// Token: 0x04001AB4 RID: 6836
		public int ArrowHeight;

		// Token: 0x04001AB5 RID: 6837
		public LabelStyle LabelStyle;

		// Token: 0x04001AB6 RID: 6838
		public int HorizontalPadding = 8;

		// Token: 0x04001AB7 RID: 6839
		public int HorizontalRowPadding;

		// Token: 0x04001AB8 RID: 6840
		public FileDropdownBoxStyle.DropdownBoxAlign PanelAlign = FileDropdownBoxStyle.DropdownBoxAlign.Bottom;

		// Token: 0x04001AB9 RID: 6841
		public int PanelOffset = 5;

		// Token: 0x02000D11 RID: 3345
		public enum DropdownBoxAlign
		{
			// Token: 0x040040A8 RID: 16552
			Top,
			// Token: 0x040040A9 RID: 16553
			Bottom,
			// Token: 0x040040AA RID: 16554
			Left,
			// Token: 0x040040AB RID: 16555
			Right
		}
	}
}
