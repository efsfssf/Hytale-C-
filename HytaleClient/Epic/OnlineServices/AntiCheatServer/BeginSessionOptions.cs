using System;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200067C RID: 1660
	public struct BeginSessionOptions
	{
		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x0003FD07 File Offset: 0x0003DF07
		// (set) Token: 0x06002B38 RID: 11064 RVA: 0x0003FD0F File Offset: 0x0003DF0F
		public uint RegisterTimeoutSeconds { get; set; }

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x0003FD18 File Offset: 0x0003DF18
		// (set) Token: 0x06002B3A RID: 11066 RVA: 0x0003FD20 File Offset: 0x0003DF20
		public Utf8String ServerName { get; set; }

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x0003FD29 File Offset: 0x0003DF29
		// (set) Token: 0x06002B3C RID: 11068 RVA: 0x0003FD31 File Offset: 0x0003DF31
		public bool EnableGameplayData { get; set; }

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x0003FD3A File Offset: 0x0003DF3A
		// (set) Token: 0x06002B3E RID: 11070 RVA: 0x0003FD42 File Offset: 0x0003DF42
		public ProductUserId LocalUserId { get; set; }
	}
}
