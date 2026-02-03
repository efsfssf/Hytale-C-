using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000284 RID: 644
	public struct CopyUserTokenByIndexOptions
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x0001A7AF File Offset: 0x000189AF
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x0001A7B7 File Offset: 0x000189B7
		public uint UserTokenIndex { get; set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x0001A7C0 File Offset: 0x000189C0
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x0001A7C8 File Offset: 0x000189C8
		public uint QueryId { get; set; }
	}
}
