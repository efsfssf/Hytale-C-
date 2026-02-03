using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B1 RID: 1713
	public struct LogPlayerSpawnOptions
	{
		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x0004133E File Offset: 0x0003F53E
		// (set) Token: 0x06002C22 RID: 11298 RVA: 0x00041346 File Offset: 0x0003F546
		public IntPtr SpawnedPlayerHandle { get; set; }

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06002C23 RID: 11299 RVA: 0x0004134F File Offset: 0x0003F54F
		// (set) Token: 0x06002C24 RID: 11300 RVA: 0x00041357 File Offset: 0x0003F557
		public uint TeamId { get; set; }

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06002C25 RID: 11301 RVA: 0x00041360 File Offset: 0x0003F560
		// (set) Token: 0x06002C26 RID: 11302 RVA: 0x00041368 File Offset: 0x0003F568
		public uint CharacterId { get; set; }
	}
}
