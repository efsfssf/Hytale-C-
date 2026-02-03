using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006B7 RID: 1719
	public struct LogPlayerUseAbilityOptions
	{
		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x00041BE2 File Offset: 0x0003FDE2
		// (set) Token: 0x06002C82 RID: 11394 RVA: 0x00041BEA File Offset: 0x0003FDEA
		public IntPtr PlayerHandle { get; set; }

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x00041BF3 File Offset: 0x0003FDF3
		// (set) Token: 0x06002C84 RID: 11396 RVA: 0x00041BFB File Offset: 0x0003FDFB
		public uint AbilityId { get; set; }

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x00041C04 File Offset: 0x0003FE04
		// (set) Token: 0x06002C86 RID: 11398 RVA: 0x00041C0C File Offset: 0x0003FE0C
		public uint AbilityDurationMs { get; set; }

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x00041C15 File Offset: 0x0003FE15
		// (set) Token: 0x06002C88 RID: 11400 RVA: 0x00041C1D File Offset: 0x0003FE1D
		public uint AbilityCooldownMs { get; set; }
	}
}
