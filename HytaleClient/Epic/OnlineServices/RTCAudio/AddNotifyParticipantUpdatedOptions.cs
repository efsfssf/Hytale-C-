using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000200 RID: 512
	public struct AddNotifyParticipantUpdatedOptions
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x000159FA File Offset: 0x00013BFA
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00015A02 File Offset: 0x00013C02
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00015A0B File Offset: 0x00013C0B
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x00015A13 File Offset: 0x00013C13
		public Utf8String RoomName { get; set; }
	}
}
