using System;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000619 RID: 1561
	public struct QueryProductUserIdMappingsOptions
	{
		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x0003B20E File Offset: 0x0003940E
		// (set) Token: 0x06002858 RID: 10328 RVA: 0x0003B216 File Offset: 0x00039416
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x0003B21F File Offset: 0x0003941F
		// (set) Token: 0x0600285A RID: 10330 RVA: 0x0003B227 File Offset: 0x00039427
		internal ExternalAccountType AccountIdType_DEPRECATED { get; set; }

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x0003B230 File Offset: 0x00039430
		// (set) Token: 0x0600285C RID: 10332 RVA: 0x0003B238 File Offset: 0x00039438
		public ProductUserId[] ProductUserIds { get; set; }
	}
}
