using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000393 RID: 915
	public struct HardMuteMemberOptions
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x00023EBB File Offset: 0x000220BB
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x00023EC3 File Offset: 0x000220C3
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x00023ECC File Offset: 0x000220CC
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x00023ED4 File Offset: 0x000220D4
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x00023EDD File Offset: 0x000220DD
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x00023EE5 File Offset: 0x000220E5
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x00023EEE File Offset: 0x000220EE
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x00023EF6 File Offset: 0x000220F6
		public bool HardMute { get; set; }
	}
}
