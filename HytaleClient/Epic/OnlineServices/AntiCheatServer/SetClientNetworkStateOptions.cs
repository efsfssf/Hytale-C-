using System;

namespace Epic.OnlineServices.AntiCheatServer
{
	// Token: 0x0200068E RID: 1678
	public struct SetClientNetworkStateOptions
	{
		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06002B94 RID: 11156 RVA: 0x000402F6 File Offset: 0x0003E4F6
		// (set) Token: 0x06002B95 RID: 11157 RVA: 0x000402FE File Offset: 0x0003E4FE
		public IntPtr ClientHandle { get; set; }

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06002B96 RID: 11158 RVA: 0x00040307 File Offset: 0x0003E507
		// (set) Token: 0x06002B97 RID: 11159 RVA: 0x0004030F File Offset: 0x0003E50F
		public bool IsNetworkActive { get; set; }
	}
}
