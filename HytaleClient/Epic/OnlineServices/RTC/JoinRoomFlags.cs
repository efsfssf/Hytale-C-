using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B9 RID: 441
	[Flags]
	public enum JoinRoomFlags : uint
	{
		// Token: 0x040005DF RID: 1503
		None = 0U,
		// Token: 0x040005E0 RID: 1504
		EnableEcho = 1U,
		// Token: 0x040005E1 RID: 1505
		EnableDatachannel = 4U
	}
}
