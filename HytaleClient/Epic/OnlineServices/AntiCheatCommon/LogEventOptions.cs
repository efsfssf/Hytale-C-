using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A3 RID: 1699
	public struct LogEventOptions
	{
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x0004053E File Offset: 0x0003E73E
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x00040546 File Offset: 0x0003E746
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x0004054F File Offset: 0x0003E74F
		// (set) Token: 0x06002BB3 RID: 11187 RVA: 0x00040557 File Offset: 0x0003E757
		public uint EventId { get; set; }

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x00040560 File Offset: 0x0003E760
		// (set) Token: 0x06002BB5 RID: 11189 RVA: 0x00040568 File Offset: 0x0003E768
		public LogEventParamPair[] Params { get; set; }
	}
}
