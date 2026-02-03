using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000830 RID: 2096
	[UIMarkupData]
	public class ColorPickerDropdownBoxStyle
	{
		// Token: 0x04001A70 RID: 6768
		public ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground Background;

		// Token: 0x04001A71 RID: 6769
		public ColorPickerStyle ColorPickerStyle;

		// Token: 0x04001A72 RID: 6770
		public ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground Overlay;

		// Token: 0x04001A73 RID: 6771
		public ColorPickerDropdownBoxStyle.ColorPickerDropdownBoxStateBackground ArrowBackground;

		// Token: 0x04001A74 RID: 6772
		public Anchor ArrowAnchor;

		// Token: 0x04001A75 RID: 6773
		public ButtonSounds Sounds;

		// Token: 0x04001A76 RID: 6774
		public PatchStyle PanelBackground;

		// Token: 0x04001A77 RID: 6775
		public int PanelWidth = 300;

		// Token: 0x04001A78 RID: 6776
		public int PanelHeight = 300;

		// Token: 0x04001A79 RID: 6777
		public Padding PanelPadding;

		// Token: 0x04001A7A RID: 6778
		public int PanelOffset;

		// Token: 0x02000D0E RID: 3342
		[UIMarkupData]
		public class ColorPickerDropdownBoxStateBackground
		{
			// Token: 0x04004097 RID: 16535
			public PatchStyle Default;

			// Token: 0x04004098 RID: 16536
			public PatchStyle Hovered;

			// Token: 0x04004099 RID: 16537
			public PatchStyle Pressed;
		}
	}
}
