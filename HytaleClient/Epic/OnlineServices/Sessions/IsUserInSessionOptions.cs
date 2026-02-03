using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010C RID: 268
	public struct IsUserInSessionOptions
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0000CAF2 File Offset: 0x0000ACF2
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0000CAFA File Offset: 0x0000ACFA
		public Utf8String SessionName { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0000CB03 File Offset: 0x0000AD03
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0000CB0B File Offset: 0x0000AD0B
		public ProductUserId TargetUserId { get; set; }
	}
}
