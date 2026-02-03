using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000512 RID: 1298
	public struct CopyItemByIdOptions
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000320CF File Offset: 0x000302CF
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x000320D7 File Offset: 0x000302D7
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000320E0 File Offset: 0x000302E0
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x000320E8 File Offset: 0x000302E8
		public Utf8String ItemId { get; set; }
	}
}
