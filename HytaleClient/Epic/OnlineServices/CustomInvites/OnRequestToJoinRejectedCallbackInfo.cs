using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005A3 RID: 1443
	public struct OnRequestToJoinRejectedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x000372BA File Offset: 0x000354BA
		// (set) Token: 0x06002577 RID: 9591 RVA: 0x000372C2 File Offset: 0x000354C2
		public object ClientData { get; set; }

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x000372CB File Offset: 0x000354CB
		// (set) Token: 0x06002579 RID: 9593 RVA: 0x000372D3 File Offset: 0x000354D3
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x000372DC File Offset: 0x000354DC
		// (set) Token: 0x0600257B RID: 9595 RVA: 0x000372E4 File Offset: 0x000354E4
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x0600257C RID: 9596 RVA: 0x000372F0 File Offset: 0x000354F0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x0003730B File Offset: 0x0003550B
		internal void Set(ref OnRequestToJoinRejectedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
