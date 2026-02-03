using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000524 RID: 1316
	public struct CopyTransactionByIndexOptions
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x000328DA File Offset: 0x00030ADA
		// (set) Token: 0x06002273 RID: 8819 RVA: 0x000328E2 File Offset: 0x00030AE2
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x000328EB File Offset: 0x00030AEB
		// (set) Token: 0x06002275 RID: 8821 RVA: 0x000328F3 File Offset: 0x00030AF3
		public uint TransactionIndex { get; set; }
	}
}
