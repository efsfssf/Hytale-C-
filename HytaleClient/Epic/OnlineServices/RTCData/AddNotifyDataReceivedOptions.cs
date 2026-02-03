using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DA RID: 474
	public struct AddNotifyDataReceivedOptions
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x00014242 File Offset: 0x00012442
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x0001424A File Offset: 0x0001244A
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00014253 File Offset: 0x00012453
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x0001425B File Offset: 0x0001245B
		public Utf8String RoomName { get; set; }
	}
}
