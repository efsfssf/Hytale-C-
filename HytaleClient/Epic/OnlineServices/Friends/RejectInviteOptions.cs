using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004F9 RID: 1273
	public struct RejectInviteOptions
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06002108 RID: 8456 RVA: 0x0003055B File Offset: 0x0002E75B
		// (set) Token: 0x06002109 RID: 8457 RVA: 0x00030563 File Offset: 0x0002E763
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x0003056C File Offset: 0x0002E76C
		// (set) Token: 0x0600210B RID: 8459 RVA: 0x00030574 File Offset: 0x0002E774
		public EpicAccountId TargetUserId { get; set; }
	}
}
