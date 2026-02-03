using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001FE RID: 510
	public struct AddNotifyAudioOutputStateOptions
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0001592E File Offset: 0x00013B2E
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x00015936 File Offset: 0x00013B36
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0001593F File Offset: 0x00013B3F
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00015947 File Offset: 0x00013B47
		public Utf8String RoomName { get; set; }
	}
}
