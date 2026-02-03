using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000270 RID: 624
	public struct UpdateParticipantVolumeCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x000192BD File Offset: 0x000174BD
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x000192C5 File Offset: 0x000174C5
		public Result ResultCode { get; set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x000192CE File Offset: 0x000174CE
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x000192D6 File Offset: 0x000174D6
		public object ClientData { get; set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000192DF File Offset: 0x000174DF
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x000192E7 File Offset: 0x000174E7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x000192F0 File Offset: 0x000174F0
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x000192F8 File Offset: 0x000174F8
		public Utf8String RoomName { get; set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00019301 File Offset: 0x00017501
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x00019309 File Offset: 0x00017509
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x00019312 File Offset: 0x00017512
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x0001931A File Offset: 0x0001751A
		public float Volume { get; set; }

		// Token: 0x0600115A RID: 4442 RVA: 0x00019324 File Offset: 0x00017524
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00019344 File Offset: 0x00017544
		internal void Set(ref UpdateParticipantVolumeCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Volume = other.Volume;
		}
	}
}
