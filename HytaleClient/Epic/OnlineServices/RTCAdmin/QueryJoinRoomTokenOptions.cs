using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000294 RID: 660
	public struct QueryJoinRoomTokenOptions
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x0001AE68 File Offset: 0x00019068
		// (set) Token: 0x06001281 RID: 4737 RVA: 0x0001AE70 File Offset: 0x00019070
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001282 RID: 4738 RVA: 0x0001AE79 File Offset: 0x00019079
		// (set) Token: 0x06001283 RID: 4739 RVA: 0x0001AE81 File Offset: 0x00019081
		public Utf8String RoomName { get; set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x0001AE8A File Offset: 0x0001908A
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x0001AE92 File Offset: 0x00019092
		public ProductUserId[] TargetUserIds { get; set; }

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001286 RID: 4742 RVA: 0x0001AE9B File Offset: 0x0001909B
		// (set) Token: 0x06001287 RID: 4743 RVA: 0x0001AEA3 File Offset: 0x000190A3
		public Utf8String TargetUserIpAddresses { get; set; }
	}
}
