using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015C RID: 348
	public struct SessionInviteReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0000ED78 File Offset: 0x0000CF78
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0000ED80 File Offset: 0x0000CF80
		public object ClientData { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0000ED89 File Offset: 0x0000CF89
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0000ED91 File Offset: 0x0000CF91
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0000ED9A File Offset: 0x0000CF9A
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0000EDA2 File Offset: 0x0000CFA2
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0000EDAB File Offset: 0x0000CFAB
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0000EDB3 File Offset: 0x0000CFB3
		public Utf8String InviteId { get; set; }

		// Token: 0x06000A90 RID: 2704 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0000EDD7 File Offset: 0x0000CFD7
		internal void Set(ref SessionInviteReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.InviteId = other.InviteId;
		}
	}
}
