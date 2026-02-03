using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000538 RID: 1336
	public struct GetOfferItemCountOptions
	{
		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x00033C6A File Offset: 0x00031E6A
		// (set) Token: 0x060022F7 RID: 8951 RVA: 0x00033C72 File Offset: 0x00031E72
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x00033C7B File Offset: 0x00031E7B
		// (set) Token: 0x060022F9 RID: 8953 RVA: 0x00033C83 File Offset: 0x00031E83
		public Utf8String OfferId { get; set; }
	}
}
