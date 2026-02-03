using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000522 RID: 1314
	public struct CopyTransactionByIdOptions
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x0003280B File Offset: 0x00030A0B
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x00032813 File Offset: 0x00030A13
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x0003281C File Offset: 0x00030A1C
		// (set) Token: 0x0600226C RID: 8812 RVA: 0x00032824 File Offset: 0x00030A24
		public Utf8String TransactionId { get; set; }
	}
}
