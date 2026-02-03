using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000595 RID: 1429
	public struct OnCustomInviteReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x00036DCC File Offset: 0x00034FCC
		// (set) Token: 0x06002521 RID: 9505 RVA: 0x00036DD4 File Offset: 0x00034FD4
		public object ClientData { get; set; }

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x00036DDD File Offset: 0x00034FDD
		// (set) Token: 0x06002523 RID: 9507 RVA: 0x00036DE5 File Offset: 0x00034FE5
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x00036DEE File Offset: 0x00034FEE
		// (set) Token: 0x06002525 RID: 9509 RVA: 0x00036DF6 File Offset: 0x00034FF6
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x00036DFF File Offset: 0x00034FFF
		// (set) Token: 0x06002527 RID: 9511 RVA: 0x00036E07 File Offset: 0x00035007
		public Utf8String CustomInviteId { get; set; }

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x00036E10 File Offset: 0x00035010
		// (set) Token: 0x06002529 RID: 9513 RVA: 0x00036E18 File Offset: 0x00035018
		public Utf8String Payload { get; set; }

		// Token: 0x0600252A RID: 9514 RVA: 0x00036E24 File Offset: 0x00035024
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x00036E40 File Offset: 0x00035040
		internal void Set(ref OnCustomInviteReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}
	}
}
