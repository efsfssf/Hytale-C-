using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000286 RID: 646
	public struct CopyUserTokenByUserIdOptions
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0001A85A File Offset: 0x00018A5A
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x0001A862 File Offset: 0x00018A62
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x0001A86B File Offset: 0x00018A6B
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x0001A873 File Offset: 0x00018A73
		public uint QueryId { get; set; }
	}
}
