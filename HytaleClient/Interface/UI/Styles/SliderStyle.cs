using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083E RID: 2110
	[UIMarkupData]
	public class SliderStyle
	{
		// Token: 0x06003AB0 RID: 15024 RVA: 0x00085EB8 File Offset: 0x000840B8
		public static SliderStyle MakeDefault()
		{
			return new SliderStyle
			{
				Background = new PatchStyle(UInt32Color.FromRGBA(1145324748U)),
				Fill = new PatchStyle(UInt32Color.FromRGBA(1717987020U)),
				Handle = new PatchStyle(UInt32Color.FromRGBA(3435973887U)),
				HandleHeight = 16,
				HandleWidth = 16
			};
		}

		// Token: 0x04001AFA RID: 6906
		public PatchStyle Background;

		// Token: 0x04001AFB RID: 6907
		public PatchStyle Fill;

		// Token: 0x04001AFC RID: 6908
		public PatchStyle Handle;

		// Token: 0x04001AFD RID: 6909
		public int HandleWidth;

		// Token: 0x04001AFE RID: 6910
		public int HandleHeight;

		// Token: 0x04001AFF RID: 6911
		public ButtonSounds Sounds;
	}
}
