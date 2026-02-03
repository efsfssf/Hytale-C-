using System;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004FD RID: 1277
	public struct SendInviteOptions
	{
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x00030877 File Offset: 0x0002EA77
		// (set) Token: 0x06002129 RID: 8489 RVA: 0x0003087F File Offset: 0x0002EA7F
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x00030888 File Offset: 0x0002EA88
		// (set) Token: 0x0600212B RID: 8491 RVA: 0x00030890 File Offset: 0x0002EA90
		public EpicAccountId TargetUserId { get; set; }
	}
}
