using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DE RID: 478
	public struct DataReceivedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x000143DA File Offset: 0x000125DA
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x000143E2 File Offset: 0x000125E2
		public object ClientData { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x000143EB File Offset: 0x000125EB
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x000143F3 File Offset: 0x000125F3
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x000143FC File Offset: 0x000125FC
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x00014404 File Offset: 0x00012604
		public Utf8String RoomName { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x0001440D File Offset: 0x0001260D
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x00014415 File Offset: 0x00012615
		public ArraySegment<byte> Data { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0001441E File Offset: 0x0001261E
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00014426 File Offset: 0x00012626
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x06000DDB RID: 3547 RVA: 0x00014430 File Offset: 0x00012630
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0001444C File Offset: 0x0001264C
		internal void Set(ref DataReceivedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Data = other.Data;
			this.ParticipantId = other.ParticipantId;
		}
	}
}
