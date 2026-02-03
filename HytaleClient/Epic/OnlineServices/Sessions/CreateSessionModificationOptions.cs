using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000FA RID: 250
	public struct CreateSessionModificationOptions
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0000C289 File Offset: 0x0000A489
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0000C291 File Offset: 0x0000A491
		public Utf8String SessionName { get; set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0000C29A File Offset: 0x0000A49A
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x0000C2A2 File Offset: 0x0000A4A2
		public Utf8String BucketId { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0000C2AB File Offset: 0x0000A4AB
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x0000C2B3 File Offset: 0x0000A4B3
		public uint MaxPlayers { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0000C2CD File Offset: 0x0000A4CD
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0000C2D5 File Offset: 0x0000A4D5
		public bool PresenceEnabled { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0000C2DE File Offset: 0x0000A4DE
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0000C2E6 File Offset: 0x0000A4E6
		public Utf8String SessionId { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0000C2EF File Offset: 0x0000A4EF
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public bool SanctionsEnabled { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0000C300 File Offset: 0x0000A500
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0000C308 File Offset: 0x0000A508
		public uint[] AllowedPlatformIds { get; set; }
	}
}
