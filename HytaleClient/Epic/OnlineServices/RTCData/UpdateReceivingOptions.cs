using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F0 RID: 496
	public struct UpdateReceivingOptions
	{
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00015104 File Offset: 0x00013304
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x0001510C File Offset: 0x0001330C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00015115 File Offset: 0x00013315
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x0001511D File Offset: 0x0001331D
		public Utf8String RoomName { get; set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00015126 File Offset: 0x00013326
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x0001512E File Offset: 0x0001332E
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00015137 File Offset: 0x00013337
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x0001513F File Offset: 0x0001333F
		public bool DataEnabled { get; set; }
	}
}
