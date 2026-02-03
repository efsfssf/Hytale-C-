using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000514 RID: 1300
	public struct CopyItemImageInfoByIndexOptions
	{
		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x0003219E File Offset: 0x0003039E
		// (set) Token: 0x0600221F RID: 8735 RVA: 0x000321A6 File Offset: 0x000303A6
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x000321AF File Offset: 0x000303AF
		// (set) Token: 0x06002221 RID: 8737 RVA: 0x000321B7 File Offset: 0x000303B7
		public Utf8String ItemId { get; set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x000321C0 File Offset: 0x000303C0
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x000321C8 File Offset: 0x000303C8
		public uint ImageInfoIndex { get; set; }
	}
}
