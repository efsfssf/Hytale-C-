using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015A RID: 346
	public struct SessionInviteAcceptedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0000EA74 File Offset: 0x0000CC74
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0000EA7C File Offset: 0x0000CC7C
		public object ClientData { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0000EA85 File Offset: 0x0000CC85
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0000EA8D File Offset: 0x0000CC8D
		public Utf8String SessionId { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0000EA96 File Offset: 0x0000CC96
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0000EA9E File Offset: 0x0000CC9E
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0000EAA7 File Offset: 0x0000CCA7
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0000EAAF File Offset: 0x0000CCAF
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		public Utf8String InviteId { get; set; }

		// Token: 0x06000A77 RID: 2679 RVA: 0x0000EACC File Offset: 0x0000CCCC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		internal void Set(ref SessionInviteAcceptedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.SessionId = other.SessionId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.InviteId = other.InviteId;
		}
	}
}
