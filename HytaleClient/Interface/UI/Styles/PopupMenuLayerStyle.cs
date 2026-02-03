using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083C RID: 2108
	[UIMarkupData]
	internal class PopupMenuLayerStyle
	{
		// Token: 0x04001AE5 RID: 6885
		public PatchStyle Background;

		// Token: 0x04001AE6 RID: 6886
		public int Padding = 2;

		// Token: 0x04001AE7 RID: 6887
		public int BaseHeight = 31;

		// Token: 0x04001AE8 RID: 6888
		public int MaxWidth = 200;

		// Token: 0x04001AE9 RID: 6889
		public int RowHeight = 25;

		// Token: 0x04001AEA RID: 6890
		public LabelStyle TitleStyle;

		// Token: 0x04001AEB RID: 6891
		public PatchStyle TitleBackground;

		// Token: 0x04001AEC RID: 6892
		public LabelStyle ItemLabelStyle;

		// Token: 0x04001AED RID: 6893
		public Padding ItemPadding;

		// Token: 0x04001AEE RID: 6894
		public PatchStyle ItemBackground;

		// Token: 0x04001AEF RID: 6895
		public int ItemIconSize = 16;

		// Token: 0x04001AF0 RID: 6896
		public PatchStyle HoveredItemBackground;

		// Token: 0x04001AF1 RID: 6897
		public PatchStyle PressedItemBackground;

		// Token: 0x04001AF2 RID: 6898
		public ButtonSounds ItemSounds;
	}
}
