using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006FE RID: 1790
	public struct UnprotectMessageOptions
	{
		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06002E0F RID: 11791 RVA: 0x00043FA8 File Offset: 0x000421A8
		// (set) Token: 0x06002E10 RID: 11792 RVA: 0x00043FB0 File Offset: 0x000421B0
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x00043FB9 File Offset: 0x000421B9
		// (set) Token: 0x06002E12 RID: 11794 RVA: 0x00043FC1 File Offset: 0x000421C1
		public uint OutBufferSizeBytes { get; set; }
	}
}
