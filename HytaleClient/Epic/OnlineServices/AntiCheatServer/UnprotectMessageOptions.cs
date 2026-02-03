using System;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x02000690 RID: 1680
	public struct UnprotectMessageOptions
	{
		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06002B9D RID: 11165 RVA: 0x000403B2 File Offset: 0x0003E5B2
		// (set) Token: 0x06002B9E RID: 11166 RVA: 0x000403BA File Offset: 0x0003E5BA
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000403C3 File Offset: 0x0003E5C3
		// (set) Token: 0x06002BA0 RID: 11168 RVA: 0x000403CB File Offset: 0x0003E5CB
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x000403D4 File Offset: 0x0003E5D4
		// (set) Token: 0x06002BA2 RID: 11170 RVA: 0x000403DC File Offset: 0x0003E5DC
		public uint OutBufferSizeBytes { get; set; }
	}
}
