using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D0 RID: 464
	public struct ParticipantStatusChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00013548 File Offset: 0x00011748
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x00013550 File Offset: 0x00011750
		public object ClientData { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00013559 File Offset: 0x00011759
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00013561 File Offset: 0x00011761
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x0001356A File Offset: 0x0001176A
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00013572 File Offset: 0x00011772
		public Utf8String RoomName { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x0001357B File Offset: 0x0001177B
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00013583 File Offset: 0x00011783
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x0001358C File Offset: 0x0001178C
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00013594 File Offset: 0x00011794
		public RTCParticipantStatus ParticipantStatus { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0001359D File Offset: 0x0001179D
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x000135A5 File Offset: 0x000117A5
		public ParticipantMetadata[] ParticipantMetadata { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x000135AE File Offset: 0x000117AE
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x000135B6 File Offset: 0x000117B6
		public bool ParticipantInBlocklist { get; set; }

		// Token: 0x06000D66 RID: 3430 RVA: 0x000135C0 File Offset: 0x000117C0
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000135DC File Offset: 0x000117DC
		internal void Set(ref ParticipantStatusChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.ParticipantStatus = other.ParticipantStatus;
			this.ParticipantMetadata = other.ParticipantMetadata;
			this.ParticipantInBlocklist = other.ParticipantInBlocklist;
		}
	}
}
