using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000510 RID: 1296
	public struct CopyEntitlementByNameAndIndexOptions
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00031FC6 File Offset: 0x000301C6
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x00031FCE File Offset: 0x000301CE
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x00031FD7 File Offset: 0x000301D7
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x00031FDF File Offset: 0x000301DF
		public Utf8String EntitlementName { get; set; }

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x00031FE8 File Offset: 0x000301E8
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x00031FF0 File Offset: 0x000301F0
		public uint Index { get; set; }
	}
}
