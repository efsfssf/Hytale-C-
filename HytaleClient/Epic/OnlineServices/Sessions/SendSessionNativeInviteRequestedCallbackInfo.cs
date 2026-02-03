using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000148 RID: 328
	public struct SendSessionNativeInviteRequestedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0000DC43 File Offset: 0x0000BE43
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0000DC4B File Offset: 0x0000BE4B
		public object ClientData { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0000DC54 File Offset: 0x0000BE54
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		public ulong UiEventId { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0000DC65 File Offset: 0x0000BE65
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0000DC6D File Offset: 0x0000BE6D
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0000DC76 File Offset: 0x0000BE76
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0000DC7E File Offset: 0x0000BE7E
		public Utf8String TargetNativeAccountType { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0000DC87 File Offset: 0x0000BE87
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0000DC8F File Offset: 0x0000BE8F
		public Utf8String TargetUserNativeAccountId { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0000DC98 File Offset: 0x0000BE98
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		public Utf8String SessionId { get; set; }

		// Token: 0x060009F6 RID: 2550 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
		internal void Set(ref SendSessionNativeInviteRequestedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.SessionId = other.SessionId;
		}
	}
}
