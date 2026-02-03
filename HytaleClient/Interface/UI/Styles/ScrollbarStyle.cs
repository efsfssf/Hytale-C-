using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083D RID: 2109
	[UIMarkupData]
	public class ScrollbarStyle
	{
		// Token: 0x06003AAE RID: 15022 RVA: 0x00085E20 File Offset: 0x00084020
		public static ScrollbarStyle MakeDefault()
		{
			return new ScrollbarStyle
			{
				Background = new PatchStyle
				{
					Color = UInt32Color.FromRGBA(1145324748U)
				},
				Handle = new PatchStyle
				{
					Color = UInt32Color.FromRGBA(3435973887U)
				},
				HoveredHandle = new PatchStyle
				{
					Color = UInt32Color.FromRGBA(4008636159U)
				},
				DraggedHandle = new PatchStyle
				{
					Color = UInt32Color.FromRGBA(2863311615U)
				}
			};
		}

		// Token: 0x04001AF3 RID: 6899
		public int Size = 20;

		// Token: 0x04001AF4 RID: 6900
		public int Spacing = 20;

		// Token: 0x04001AF5 RID: 6901
		public bool OnlyVisibleWhenHovered;

		// Token: 0x04001AF6 RID: 6902
		public PatchStyle Background;

		// Token: 0x04001AF7 RID: 6903
		public PatchStyle Handle;

		// Token: 0x04001AF8 RID: 6904
		public PatchStyle HoveredHandle;

		// Token: 0x04001AF9 RID: 6905
		public PatchStyle DraggedHandle;
	}
}
