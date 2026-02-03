using System;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A7 RID: 423
	public struct QueryActivePlayerSanctionsOptions
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00011E27 File Offset: 0x00010027
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x00011E2F File Offset: 0x0001002F
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00011E38 File Offset: 0x00010038
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x00011E40 File Offset: 0x00010040
		public ProductUserId LocalUserId { get; set; }
	}
}
