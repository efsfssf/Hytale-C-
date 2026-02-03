using System;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD6 RID: 2774
	internal class MouseSettings
	{
		// Token: 0x0600577B RID: 22395 RVA: 0x001A8858 File Offset: 0x001A6A58
		public MouseSettings Clone()
		{
			return new MouseSettings
			{
				MouseRawInputMode = this.MouseRawInputMode,
				MouseInverted = this.MouseInverted,
				MouseXSpeed = this.MouseXSpeed,
				MouseYSpeed = this.MouseYSpeed
			};
		}

		// Token: 0x04003571 RID: 13681
		public bool MouseRawInputMode = false;

		// Token: 0x04003572 RID: 13682
		public bool MouseInverted = false;

		// Token: 0x04003573 RID: 13683
		public float MouseXSpeed = 3.5f;

		// Token: 0x04003574 RID: 13684
		public float MouseYSpeed = 3.5f;
	}
}
