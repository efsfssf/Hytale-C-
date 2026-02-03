using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001FC RID: 508
	public struct AddNotifyAudioInputStateOptions
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00015860 File Offset: 0x00013A60
		// (set) Token: 0x06000EAE RID: 3758 RVA: 0x00015868 File Offset: 0x00013A68
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000EAF RID: 3759 RVA: 0x00015871 File Offset: 0x00013A71
		// (set) Token: 0x06000EB0 RID: 3760 RVA: 0x00015879 File Offset: 0x00013A79
		public Utf8String RoomName { get; set; }
	}
}
