using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006F4 RID: 1780
	public struct ProtectMessageOptions
	{
		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x00043B41 File Offset: 0x00041D41
		// (set) Token: 0x06002DDD RID: 11741 RVA: 0x00043B49 File Offset: 0x00041D49
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06002DDE RID: 11742 RVA: 0x00043B52 File Offset: 0x00041D52
		// (set) Token: 0x06002DDF RID: 11743 RVA: 0x00043B5A File Offset: 0x00041D5A
		public uint OutBufferSizeBytes { get; set; }
	}
}
