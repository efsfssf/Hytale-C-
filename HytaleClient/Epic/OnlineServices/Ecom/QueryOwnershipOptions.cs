using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000563 RID: 1379
	public struct QueryOwnershipOptions
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x00034F63 File Offset: 0x00033163
		// (set) Token: 0x060023FD RID: 9213 RVA: 0x00034F6B File Offset: 0x0003316B
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x00034F74 File Offset: 0x00033174
		// (set) Token: 0x060023FF RID: 9215 RVA: 0x00034F7C File Offset: 0x0003317C
		public Utf8String[] CatalogItemIds { get; set; }

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x00034F85 File Offset: 0x00033185
		// (set) Token: 0x06002401 RID: 9217 RVA: 0x00034F8D File Offset: 0x0003318D
		public Utf8String CatalogNamespace { get; set; }
	}
}
