using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E8 RID: 488
	public struct ParticipantUpdatedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x000146E8 File Offset: 0x000128E8
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x000146F0 File Offset: 0x000128F0
		public object ClientData { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x000146F9 File Offset: 0x000128F9
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x00014701 File Offset: 0x00012901
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0001470A File Offset: 0x0001290A
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x00014712 File Offset: 0x00012912
		public Utf8String RoomName { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0001471B File Offset: 0x0001291B
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x00014723 File Offset: 0x00012923
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0001472C File Offset: 0x0001292C
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x00014734 File Offset: 0x00012934
		public RTCDataStatus DataStatus { get; set; }

		// Token: 0x06000E16 RID: 3606 RVA: 0x00014740 File Offset: 0x00012940
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0001475C File Offset: 0x0001295C
		internal void Set(ref ParticipantUpdatedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.DataStatus = other.DataStatus;
		}
	}
}
