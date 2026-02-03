using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000282 RID: 642
	public struct UpdateSendingVolumeOptions
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x0001A6A4 File Offset: 0x000188A4
		// (set) Token: 0x06001214 RID: 4628 RVA: 0x0001A6AC File Offset: 0x000188AC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x0001A6B5 File Offset: 0x000188B5
		// (set) Token: 0x06001216 RID: 4630 RVA: 0x0001A6BD File Offset: 0x000188BD
		public Utf8String RoomName { get; set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x0001A6C6 File Offset: 0x000188C6
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x0001A6CE File Offset: 0x000188CE
		public float Volume { get; set; }
	}
}
