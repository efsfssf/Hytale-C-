using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001F8 RID: 504
	public struct AddNotifyAudioBeforeSendOptions
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00015763 File Offset: 0x00013963
		// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x0001576B File Offset: 0x0001396B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x00015774 File Offset: 0x00013974
		// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x0001577C File Offset: 0x0001397C
		public Utf8String RoomName { get; set; }
	}
}
