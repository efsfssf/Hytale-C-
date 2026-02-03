using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D9 RID: 1241
	public struct GetBlockedUserAtIndexOptions
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x0002F857 File Offset: 0x0002DA57
		// (set) Token: 0x06002050 RID: 8272 RVA: 0x0002F85F File Offset: 0x0002DA5F
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x0002F868 File Offset: 0x0002DA68
		// (set) Token: 0x06002052 RID: 8274 RVA: 0x0002F870 File Offset: 0x0002DA70
		public int Index { get; set; }
	}
}
