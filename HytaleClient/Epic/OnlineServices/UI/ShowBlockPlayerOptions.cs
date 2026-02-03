using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000088 RID: 136
	public struct ShowBlockPlayerOptions
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00007D99 File Offset: 0x00005F99
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00007DA1 File Offset: 0x00005FA1
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00007DAA File Offset: 0x00005FAA
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00007DB2 File Offset: 0x00005FB2
		public EpicAccountId TargetUserId { get; set; }
	}
}
