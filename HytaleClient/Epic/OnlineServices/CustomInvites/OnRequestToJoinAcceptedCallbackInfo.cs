using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x0200059D RID: 1437
	public struct OnRequestToJoinAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002553 RID: 9555 RVA: 0x000370D0 File Offset: 0x000352D0
		// (set) Token: 0x06002554 RID: 9556 RVA: 0x000370D8 File Offset: 0x000352D8
		public object ClientData { get; set; }

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002555 RID: 9557 RVA: 0x000370E1 File Offset: 0x000352E1
		// (set) Token: 0x06002556 RID: 9558 RVA: 0x000370E9 File Offset: 0x000352E9
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x000370F2 File Offset: 0x000352F2
		// (set) Token: 0x06002558 RID: 9560 RVA: 0x000370FA File Offset: 0x000352FA
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x06002559 RID: 9561 RVA: 0x00037104 File Offset: 0x00035304
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x0003711F File Offset: 0x0003531F
		internal void Set(ref OnRequestToJoinAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
		}
	}
}
