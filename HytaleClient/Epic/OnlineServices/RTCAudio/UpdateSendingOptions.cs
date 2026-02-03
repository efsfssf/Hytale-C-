using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027E RID: 638
	public struct UpdateSendingOptions
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x0001A2CC File Offset: 0x000184CC
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x0001A2D4 File Offset: 0x000184D4
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0001A2DD File Offset: 0x000184DD
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x0001A2E5 File Offset: 0x000184E5
		public Utf8String RoomName { get; set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0001A2EE File Offset: 0x000184EE
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x0001A2F6 File Offset: 0x000184F6
		public RTCAudioStatus AudioStatus { get; set; }
	}
}
