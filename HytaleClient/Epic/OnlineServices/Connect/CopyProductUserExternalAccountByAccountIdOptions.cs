using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005CB RID: 1483
	public struct CopyProductUserExternalAccountByAccountIdOptions
	{
		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x00039309 File Offset: 0x00037509
		// (set) Token: 0x060026A0 RID: 9888 RVA: 0x00039311 File Offset: 0x00037511
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x0003931A File Offset: 0x0003751A
		// (set) Token: 0x060026A2 RID: 9890 RVA: 0x00039322 File Offset: 0x00037522
		public Utf8String AccountId { get; set; }
	}
}
