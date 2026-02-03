using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006AF RID: 1711
	public struct LogPlayerReviveOptions
	{
		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06002C18 RID: 11288 RVA: 0x0004127D File Offset: 0x0003F47D
		// (set) Token: 0x06002C19 RID: 11289 RVA: 0x00041285 File Offset: 0x0003F485
		public IntPtr RevivedPlayerHandle { get; set; }

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x0004128E File Offset: 0x0003F48E
		// (set) Token: 0x06002C1B RID: 11291 RVA: 0x00041296 File Offset: 0x0003F496
		public IntPtr ReviverPlayerHandle { get; set; }
	}
}
