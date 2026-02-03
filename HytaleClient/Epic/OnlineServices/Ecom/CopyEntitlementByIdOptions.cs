using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200050C RID: 1292
	public struct CopyEntitlementByIdOptions
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x00031E3C File Offset: 0x0003003C
		// (set) Token: 0x060021F8 RID: 8696 RVA: 0x00031E44 File Offset: 0x00030044
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x00031E4D File Offset: 0x0003004D
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x00031E55 File Offset: 0x00030055
		public Utf8String EntitlementId { get; set; }
	}
}
