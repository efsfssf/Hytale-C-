using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000112 RID: 274
	public struct JoinSessionOptions
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0000CEE1 File Offset: 0x0000B0E1
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0000CEE9 File Offset: 0x0000B0E9
		public Utf8String SessionName { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0000CEF2 File Offset: 0x0000B0F2
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x0000CEFA File Offset: 0x0000B0FA
		public SessionDetails SessionHandle { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0000CF03 File Offset: 0x0000B103
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0000CF0B File Offset: 0x0000B10B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0000CF14 File Offset: 0x0000B114
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0000CF1C File Offset: 0x0000B11C
		public bool PresenceEnabled { get; set; }
	}
}
