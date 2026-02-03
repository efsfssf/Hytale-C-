using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200028A RID: 650
	public struct KickOptions
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x0001AA69 File Offset: 0x00018C69
		// (set) Token: 0x06001241 RID: 4673 RVA: 0x0001AA71 File Offset: 0x00018C71
		public Utf8String RoomName { get; set; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x0001AA7A File Offset: 0x00018C7A
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x0001AA82 File Offset: 0x00018C82
		public ProductUserId TargetUserId { get; set; }
	}
}
