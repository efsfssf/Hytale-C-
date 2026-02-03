using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001EC RID: 492
	public struct SendDataOptions
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00014C7F File Offset: 0x00012E7F
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x00014C87 File Offset: 0x00012E87
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00014C90 File Offset: 0x00012E90
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00014C98 File Offset: 0x00012E98
		public Utf8String RoomName { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00014CA1 File Offset: 0x00012EA1
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00014CA9 File Offset: 0x00012EA9
		public ArraySegment<byte> Data { get; set; }
	}
}
