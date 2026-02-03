using System;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F4 RID: 500
	public struct UpdateSendingOptions
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00015544 File Offset: 0x00013744
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x0001554C File Offset: 0x0001374C
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00015555 File Offset: 0x00013755
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x0001555D File Offset: 0x0001375D
		public Utf8String RoomName { get; set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00015566 File Offset: 0x00013766
		// (set) Token: 0x06000E8E RID: 3726 RVA: 0x0001556E File Offset: 0x0001376E
		public bool DataEnabled { get; set; }
	}
}
