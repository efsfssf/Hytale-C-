using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B1 RID: 433
	public struct BlockParticipantCallbackInfo : ICallbackInfo
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00012316 File Offset: 0x00010516
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0001231E File Offset: 0x0001051E
		public Result ResultCode { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00012327 File Offset: 0x00010527
		// (set) Token: 0x06000C76 RID: 3190 RVA: 0x0001232F File Offset: 0x0001052F
		public object ClientData { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00012338 File Offset: 0x00010538
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x00012340 File Offset: 0x00010540
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00012349 File Offset: 0x00010549
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x00012351 File Offset: 0x00010551
		public Utf8String RoomName { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0001235A File Offset: 0x0001055A
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x00012362 File Offset: 0x00010562
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0001236B File Offset: 0x0001056B
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x00012373 File Offset: 0x00010573
		public bool Blocked { get; set; }

		// Token: 0x06000C7F RID: 3199 RVA: 0x0001237C File Offset: 0x0001057C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0001239C File Offset: 0x0001059C
		internal void Set(ref BlockParticipantCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Blocked = other.Blocked;
		}
	}
}
