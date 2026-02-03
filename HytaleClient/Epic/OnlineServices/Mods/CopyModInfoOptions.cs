using System;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200032D RID: 813
	public struct CopyModInfoOptions
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x00020808 File Offset: 0x0001EA08
		// (set) Token: 0x06001646 RID: 5702 RVA: 0x00020810 File Offset: 0x0001EA10
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x00020819 File Offset: 0x0001EA19
		// (set) Token: 0x06001648 RID: 5704 RVA: 0x00020821 File Offset: 0x0001EA21
		public ModEnumerationType Type { get; set; }
	}
}
