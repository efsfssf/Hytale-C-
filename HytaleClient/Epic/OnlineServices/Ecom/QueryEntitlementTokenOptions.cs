using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000557 RID: 1367
	public struct QueryEntitlementTokenOptions
	{
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x00034667 File Offset: 0x00032867
		// (set) Token: 0x060023A1 RID: 9121 RVA: 0x0003466F File Offset: 0x0003286F
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x00034678 File Offset: 0x00032878
		// (set) Token: 0x060023A3 RID: 9123 RVA: 0x00034680 File Offset: 0x00032880
		public Utf8String[] EntitlementNames { get; set; }
	}
}
