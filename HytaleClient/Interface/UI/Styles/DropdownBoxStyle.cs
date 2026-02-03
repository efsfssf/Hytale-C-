using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x02000833 RID: 2099
	[UIMarkupData]
	public class DropdownBoxStyle
	{
		// Token: 0x04001A85 RID: 6789
		public PatchStyle DefaultBackground;

		// Token: 0x04001A86 RID: 6790
		public PatchStyle HoveredBackground;

		// Token: 0x04001A87 RID: 6791
		public PatchStyle PressedBackground;

		// Token: 0x04001A88 RID: 6792
		public PatchStyle DisabledBackground;

		// Token: 0x04001A89 RID: 6793
		public UIPath IconTexturePath;

		// Token: 0x04001A8A RID: 6794
		public int IconWidth;

		// Token: 0x04001A8B RID: 6795
		public int IconHeight;

		// Token: 0x04001A8C RID: 6796
		public UIPath DefaultArrowTexturePath;

		// Token: 0x04001A8D RID: 6797
		public UIPath HoveredArrowTexturePath;

		// Token: 0x04001A8E RID: 6798
		public UIPath PressedArrowTexturePath;

		// Token: 0x04001A8F RID: 6799
		public UIPath DisabledArrowTexturePath;

		// Token: 0x04001A90 RID: 6800
		public int ArrowWidth;

		// Token: 0x04001A91 RID: 6801
		public int ArrowHeight;

		// Token: 0x04001A92 RID: 6802
		public int HorizontalPadding = 8;

		// Token: 0x04001A93 RID: 6803
		public LabelStyle LabelStyle;

		// Token: 0x04001A94 RID: 6804
		public LabelStyle DisabledLabelStyle;

		// Token: 0x04001A95 RID: 6805
		public DropdownBoxSounds Sounds;

		// Token: 0x04001A96 RID: 6806
		public LabelStyle PanelTitleLabelStyle;

		// Token: 0x04001A97 RID: 6807
		public int EntryHeight = 40;

		// Token: 0x04001A98 RID: 6808
		public int EntriesInViewport = 3;

		// Token: 0x04001A99 RID: 6809
		public int HorizontalEntryPadding;

		// Token: 0x04001A9A RID: 6810
		public int EntryIconHeight;

		// Token: 0x04001A9B RID: 6811
		public int EntryIconWidth;

		// Token: 0x04001A9C RID: 6812
		public PatchStyle EntryIconBackground;

		// Token: 0x04001A9D RID: 6813
		public PatchStyle SelectedEntryIconBackground;

		// Token: 0x04001A9E RID: 6814
		public LabelStyle EntryLabelStyle;

		// Token: 0x04001A9F RID: 6815
		public LabelStyle SelectedEntryLabelStyle;

		// Token: 0x04001AA0 RID: 6816
		public LabelStyle NoItemsLabelStyle;

		// Token: 0x04001AA1 RID: 6817
		public PatchStyle HoveredEntryBackground;

		// Token: 0x04001AA2 RID: 6818
		public PatchStyle PressedEntryBackground;

		// Token: 0x04001AA3 RID: 6819
		public int FocusOutlineSize;

		// Token: 0x04001AA4 RID: 6820
		public UInt32Color FocusOutlineColor;

		// Token: 0x04001AA5 RID: 6821
		public ButtonSounds EntrySounds;

		// Token: 0x04001AA6 RID: 6822
		public ScrollbarStyle PanelScrollbarStyle;

		// Token: 0x04001AA7 RID: 6823
		public PatchStyle PanelBackground;

		// Token: 0x04001AA8 RID: 6824
		public int PanelPadding;

		// Token: 0x04001AA9 RID: 6825
		public DropdownBoxStyle.DropdownBoxAlign PanelAlign = DropdownBoxStyle.DropdownBoxAlign.Bottom;

		// Token: 0x04001AAA RID: 6826
		public int PanelOffset = 5;

		// Token: 0x04001AAB RID: 6827
		public int? PanelWidth;

		// Token: 0x04001AAC RID: 6828
		public DropdownBoxStyle.DropdownBoxSearchInputStyle SearchInputStyle;

		// Token: 0x02000D0F RID: 3343
		[UIMarkupData]
		public class DropdownBoxSearchInputStyle
		{
			// Token: 0x0400409A RID: 16538
			public PatchStyle Background;

			// Token: 0x0400409B RID: 16539
			public InputFieldIcon Icon;

			// Token: 0x0400409C RID: 16540
			public InputFieldStyle Style = new InputFieldStyle();

			// Token: 0x0400409D RID: 16541
			public InputFieldStyle PlaceholderStyle = new InputFieldStyle();

			// Token: 0x0400409E RID: 16542
			public Anchor Anchor;

			// Token: 0x0400409F RID: 16543
			public Padding Padding;

			// Token: 0x040040A0 RID: 16544
			public string PlaceholderText;

			// Token: 0x040040A1 RID: 16545
			public InputFieldButtonStyle ClearButtonStyle;
		}

		// Token: 0x02000D10 RID: 3344
		public enum DropdownBoxAlign
		{
			// Token: 0x040040A3 RID: 16547
			Top,
			// Token: 0x040040A4 RID: 16548
			Bottom,
			// Token: 0x040040A5 RID: 16549
			Left,
			// Token: 0x040040A6 RID: 16550
			Right
		}
	}
}
