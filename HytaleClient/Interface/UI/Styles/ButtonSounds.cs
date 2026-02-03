using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200082E RID: 2094
	[UIMarkupData]
	public class ButtonSounds
	{
		// Token: 0x06003A92 RID: 14994 RVA: 0x00085A54 File Offset: 0x00083C54
		public ButtonSounds Clone()
		{
			return new ButtonSounds
			{
				Activate = this.Activate,
				Context = this.Context,
				MouseHover = this.MouseHover
			};
		}

		// Token: 0x04001A67 RID: 6759
		public SoundStyle Activate;

		// Token: 0x04001A68 RID: 6760
		public SoundStyle Context;

		// Token: 0x04001A69 RID: 6761
		public SoundStyle MouseHover;
	}
}
