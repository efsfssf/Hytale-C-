using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200015E RID: 350
	public struct SessionInviteRejectedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0000EFDF File Offset: 0x0000D1DF
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0000EFE7 File Offset: 0x0000D1E7
		public object ClientData { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		public Utf8String InviteId { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0000F001 File Offset: 0x0000D201
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0000F009 File Offset: 0x0000D209
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0000F012 File Offset: 0x0000D212
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0000F01A File Offset: 0x0000D21A
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0000F023 File Offset: 0x0000D223
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0000F02B File Offset: 0x0000D22B
		public Utf8String SessionId { get; set; }

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0000F034 File Offset: 0x0000D234
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000F050 File Offset: 0x0000D250
		internal void Set(ref SessionInviteRejectedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.InviteId = other.InviteId;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.SessionId = other.SessionId;
		}
	}
}
