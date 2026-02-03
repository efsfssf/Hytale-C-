using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000276 RID: 630
	public struct UpdateReceivingOptions
	{
		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00019AC4 File Offset: 0x00017CC4
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x00019ACC File Offset: 0x00017CCC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x00019AD5 File Offset: 0x00017CD5
		// (set) Token: 0x0600119E RID: 4510 RVA: 0x00019ADD File Offset: 0x00017CDD
		public Utf8String RoomName { get; set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00019AE6 File Offset: 0x00017CE6
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x00019AEE File Offset: 0x00017CEE
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x00019AF7 File Offset: 0x00017CF7
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x00019AFF File Offset: 0x00017CFF
		public bool AudioEnabled { get; set; }
	}
}
