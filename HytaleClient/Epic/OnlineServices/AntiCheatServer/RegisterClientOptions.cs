using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068C RID: 1676
	public struct RegisterClientOptions
	{
		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06002B7C RID: 11132 RVA: 0x000400CA File Offset: 0x0003E2CA
		// (set) Token: 0x06002B7D RID: 11133 RVA: 0x000400D2 File Offset: 0x0003E2D2
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000400DB File Offset: 0x0003E2DB
		// (set) Token: 0x06002B7F RID: 11135 RVA: 0x000400E3 File Offset: 0x0003E2E3
		public AntiCheatCommonClientType ClientType { get; set; }

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000400EC File Offset: 0x0003E2EC
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000400F4 File Offset: 0x0003E2F4
		public AntiCheatCommonClientPlatform ClientPlatform { get; set; }

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000400FD File Offset: 0x0003E2FD
		// (set) Token: 0x06002B83 RID: 11139 RVA: 0x00040105 File Offset: 0x0003E305
		internal Utf8String AccountId_DEPRECATED { get; set; }

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x0004010E File Offset: 0x0003E30E
		// (set) Token: 0x06002B85 RID: 11141 RVA: 0x00040116 File Offset: 0x0003E316
		public Utf8String IpAddress { get; set; }

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x0004011F File Offset: 0x0003E31F
		// (set) Token: 0x06002B87 RID: 11143 RVA: 0x00040127 File Offset: 0x0003E327
		public ProductUserId UserId { get; set; }

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06002B88 RID: 11144 RVA: 0x00040130 File Offset: 0x0003E330
		// (set) Token: 0x06002B89 RID: 11145 RVA: 0x00040138 File Offset: 0x0003E338
		public int Reserved01 { get; set; }
	}
}
