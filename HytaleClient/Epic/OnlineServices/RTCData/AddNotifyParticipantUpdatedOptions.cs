using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DC RID: 476
	public struct AddNotifyParticipantUpdatedOptions
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0001430E File Offset: 0x0001250E
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00014316 File Offset: 0x00012516
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0001431F File Offset: 0x0001251F
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x00014327 File Offset: 0x00012527
		public Utf8String RoomName { get; set; }
	}
}
