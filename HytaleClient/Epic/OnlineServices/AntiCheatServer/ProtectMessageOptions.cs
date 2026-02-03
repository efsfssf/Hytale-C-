using System;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000688 RID: 1672
	public struct ProtectMessageOptions
	{
		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x0003FEF1 File Offset: 0x0003E0F1
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x0003FEF9 File Offset: 0x0003E0F9
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x0003FF02 File Offset: 0x0003E102
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x0003FF0A File Offset: 0x0003E10A
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0003FF13 File Offset: 0x0003E113
		// (set) Token: 0x06002B6C RID: 11116 RVA: 0x0003FF1B File Offset: 0x0003E11B
		public uint OutBufferSizeBytes { get; set; }
	}
}
