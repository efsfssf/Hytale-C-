using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200083F RID: 2111
	[UIMarkupData]
	public class SoundStyle
	{
		// Token: 0x04001B00 RID: 6912
		public UIPath SoundPath;

		// Token: 0x04001B01 RID: 6913
		public float Volume;

		// Token: 0x04001B02 RID: 6914
		public float MinPitch;

		// Token: 0x04001B03 RID: 6915
		public float MaxPitch;

		// Token: 0x04001B04 RID: 6916
		public bool StopExistingPlayback;
	}
}
