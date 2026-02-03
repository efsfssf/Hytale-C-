using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000567 RID: 1383
	public struct QueryOwnershipTokenOptions
	{
		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000352D3 File Offset: 0x000334D3
		// (set) Token: 0x06002420 RID: 9248 RVA: 0x000352DB File Offset: 0x000334DB
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x000352E4 File Offset: 0x000334E4
		// (set) Token: 0x06002422 RID: 9250 RVA: 0x000352EC File Offset: 0x000334EC
		public Utf8String[] CatalogItemIds { get; set; }

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000352F5 File Offset: 0x000334F5
		// (set) Token: 0x06002424 RID: 9252 RVA: 0x000352FD File Offset: 0x000334FD
		public Utf8String CatalogNamespace { get; set; }
	}
}
