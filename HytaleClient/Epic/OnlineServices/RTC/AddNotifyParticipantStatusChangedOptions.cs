using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001AD RID: 429
	public struct AddNotifyParticipantStatusChangedOptions
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0001217E File Offset: 0x0001037E
		// (set) Token: 0x06000C62 RID: 3170 RVA: 0x00012186 File Offset: 0x00010386
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0001218F File Offset: 0x0001038F
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00012197 File Offset: 0x00010397
		public Utf8String RoomName { get; set; }
	}
}
