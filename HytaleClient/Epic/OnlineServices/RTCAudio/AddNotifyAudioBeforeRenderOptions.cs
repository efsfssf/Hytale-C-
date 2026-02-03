using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x020001F6 RID: 502
	public struct AddNotifyAudioBeforeRenderOptions
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00015653 File Offset: 0x00013853
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x0001565B File Offset: 0x0001385B
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00015664 File Offset: 0x00013864
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x0001566C File Offset: 0x0001386C
		public Utf8String RoomName { get; set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00015675 File Offset: 0x00013875
		// (set) Token: 0x06000E9A RID: 3738 RVA: 0x0001567D File Offset: 0x0001387D
		public bool UnmixedAudio { get; set; }
	}
}
