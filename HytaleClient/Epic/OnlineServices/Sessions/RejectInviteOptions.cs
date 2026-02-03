using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000142 RID: 322
	public struct RejectInviteOptions
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0000D905 File Offset: 0x0000BB05
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0000D90D File Offset: 0x0000BB0D
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0000D916 File Offset: 0x0000BB16
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0000D91E File Offset: 0x0000BB1E
		public Utf8String InviteId { get; set; }
	}
}
