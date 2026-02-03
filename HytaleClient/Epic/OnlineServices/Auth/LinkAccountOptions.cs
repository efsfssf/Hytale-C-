using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200063F RID: 1599
	public struct LinkAccountOptions
	{
		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600296D RID: 10605 RVA: 0x0003D198 File Offset: 0x0003B398
		// (set) Token: 0x0600296E RID: 10606 RVA: 0x0003D1A0 File Offset: 0x0003B3A0
		public LinkAccountFlags LinkAccountFlags { get; set; }

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x0003D1A9 File Offset: 0x0003B3A9
		// (set) Token: 0x06002970 RID: 10608 RVA: 0x0003D1B1 File Offset: 0x0003B3B1
		public ContinuanceToken ContinuanceToken { get; set; }

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06002971 RID: 10609 RVA: 0x0003D1BA File Offset: 0x0003B3BA
		// (set) Token: 0x06002972 RID: 10610 RVA: 0x0003D1C2 File Offset: 0x0003B3C2
		public EpicAccountId LocalUserId { get; set; }
	}
}
