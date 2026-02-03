using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000262 RID: 610
	public struct SendAudioOptions
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x00018C6B File Offset: 0x00016E6B
		// (set) Token: 0x06001107 RID: 4359 RVA: 0x00018C73 File Offset: 0x00016E73
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x00018C7C File Offset: 0x00016E7C
		// (set) Token: 0x06001109 RID: 4361 RVA: 0x00018C84 File Offset: 0x00016E84
		public Utf8String RoomName { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x00018C8D File Offset: 0x00016E8D
		// (set) Token: 0x0600110B RID: 4363 RVA: 0x00018C95 File Offset: 0x00016E95
		public AudioBuffer? Buffer { get; set; }
	}
}
