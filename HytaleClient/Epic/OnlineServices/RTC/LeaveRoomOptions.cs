using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BE RID: 446
	public struct LeaveRoomOptions
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x000131EF File Offset: 0x000113EF
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x000131F7 File Offset: 0x000113F7
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00013200 File Offset: 0x00011400
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00013208 File Offset: 0x00011408
		public Utf8String RoomName { get; set; }
	}
}
