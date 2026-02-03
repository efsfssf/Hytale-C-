using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001AF RID: 431
	public struct AddNotifyRoomStatisticsUpdatedOptions
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0001224A File Offset: 0x0001044A
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00012252 File Offset: 0x00010452
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0001225B File Offset: 0x0001045B
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x00012263 File Offset: 0x00010463
		public Utf8String RoomName { get; set; }
	}
}
