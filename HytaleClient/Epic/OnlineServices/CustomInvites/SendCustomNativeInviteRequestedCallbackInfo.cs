using System;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BA RID: 1466
	public struct SendCustomNativeInviteRequestedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x00037F2A File Offset: 0x0003612A
		// (set) Token: 0x06002614 RID: 9748 RVA: 0x00037F32 File Offset: 0x00036132
		public object ClientData { get; set; }

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x00037F3B File Offset: 0x0003613B
		// (set) Token: 0x06002616 RID: 9750 RVA: 0x00037F43 File Offset: 0x00036143
		public ulong UiEventId { get; set; }

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x00037F4C File Offset: 0x0003614C
		// (set) Token: 0x06002618 RID: 9752 RVA: 0x00037F54 File Offset: 0x00036154
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x00037F5D File Offset: 0x0003615D
		// (set) Token: 0x0600261A RID: 9754 RVA: 0x00037F65 File Offset: 0x00036165
		public Utf8String TargetNativeAccountType { get; set; }

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x00037F6E File Offset: 0x0003616E
		// (set) Token: 0x0600261C RID: 9756 RVA: 0x00037F76 File Offset: 0x00036176
		public Utf8String TargetUserNativeAccountId { get; set; }

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x0600261D RID: 9757 RVA: 0x00037F7F File Offset: 0x0003617F
		// (set) Token: 0x0600261E RID: 9758 RVA: 0x00037F87 File Offset: 0x00036187
		public Utf8String InviteId { get; set; }

		// Token: 0x0600261F RID: 9759 RVA: 0x00037F90 File Offset: 0x00036190
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x00037FAC File Offset: 0x000361AC
		internal void Set(ref SendCustomNativeInviteRequestedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.InviteId = other.InviteId;
		}
	}
}
