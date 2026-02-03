using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006DE RID: 1758
	public struct BeginSessionOptions
	{
		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0004363F File Offset: 0x0004183F
		// (set) Token: 0x06002D7B RID: 11643 RVA: 0x00043647 File Offset: 0x00041847
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06002D7C RID: 11644 RVA: 0x00043650 File Offset: 0x00041850
		// (set) Token: 0x06002D7D RID: 11645 RVA: 0x00043658 File Offset: 0x00041858
		public AntiCheatClientMode Mode { get; set; }
	}
}
