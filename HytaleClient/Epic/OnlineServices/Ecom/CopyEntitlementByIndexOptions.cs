using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200050E RID: 1294
	public struct CopyEntitlementByIndexOptions
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x00031F0A File Offset: 0x0003010A
		// (set) Token: 0x06002201 RID: 8705 RVA: 0x00031F12 File Offset: 0x00030112
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x00031F1B File Offset: 0x0003011B
		// (set) Token: 0x06002203 RID: 8707 RVA: 0x00031F23 File Offset: 0x00030123
		public uint EntitlementIndex { get; set; }
	}
}
