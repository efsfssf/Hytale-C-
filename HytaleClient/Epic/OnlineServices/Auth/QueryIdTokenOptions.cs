using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000661 RID: 1633
	public struct QueryIdTokenOptions
	{
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x0003E08F File Offset: 0x0003C28F
		// (set) Token: 0x06002A45 RID: 10821 RVA: 0x0003E097 File Offset: 0x0003C297
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
		// (set) Token: 0x06002A47 RID: 10823 RVA: 0x0003E0A8 File Offset: 0x0003C2A8
		public EpicAccountId TargetAccountId { get; set; }
	}
}
