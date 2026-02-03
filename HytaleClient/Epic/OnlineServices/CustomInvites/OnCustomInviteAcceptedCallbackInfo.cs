using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000591 RID: 1425
	public struct OnCustomInviteAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x00036AC8 File Offset: 0x00034CC8
		// (set) Token: 0x060024FE RID: 9470 RVA: 0x00036AD0 File Offset: 0x00034CD0
		public object ClientData { get; set; }

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x00036AD9 File Offset: 0x00034CD9
		// (set) Token: 0x06002500 RID: 9472 RVA: 0x00036AE1 File Offset: 0x00034CE1
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x00036AEA File Offset: 0x00034CEA
		// (set) Token: 0x06002502 RID: 9474 RVA: 0x00036AF2 File Offset: 0x00034CF2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06002503 RID: 9475 RVA: 0x00036AFB File Offset: 0x00034CFB
		// (set) Token: 0x06002504 RID: 9476 RVA: 0x00036B03 File Offset: 0x00034D03
		public Utf8String CustomInviteId { get; set; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x00036B0C File Offset: 0x00034D0C
		// (set) Token: 0x06002506 RID: 9478 RVA: 0x00036B14 File Offset: 0x00034D14
		public Utf8String Payload { get; set; }

		// Token: 0x06002507 RID: 9479 RVA: 0x00036B20 File Offset: 0x00034D20
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x00036B3C File Offset: 0x00034D3C
		internal void Set(ref OnCustomInviteAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}
	}
}
