using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000516 RID: 1302
	public struct CopyItemReleaseByIndexOptions
	{
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000322A7 File Offset: 0x000304A7
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x000322AF File Offset: 0x000304AF
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000322B8 File Offset: 0x000304B8
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x000322C0 File Offset: 0x000304C0
		public Utf8String ItemId { get; set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000322C9 File Offset: 0x000304C9
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x000322D1 File Offset: 0x000304D1
		public uint ReleaseIndex { get; set; }
	}
}
