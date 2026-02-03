using System;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x02000299 RID: 665
	public struct SetParticipantHardMuteOptions
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0001B391 File Offset: 0x00019591
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x0001B399 File Offset: 0x00019599
		public Utf8String RoomName { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0001B3A2 File Offset: 0x000195A2
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x0001B3AA File Offset: 0x000195AA
		public ProductUserId TargetUserId { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x0001B3B3 File Offset: 0x000195B3
		// (set) Token: 0x060012AD RID: 4781 RVA: 0x0001B3BB File Offset: 0x000195BB
		public bool Mute { get; set; }
	}
}
