using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027A RID: 634
	public struct UpdateReceivingVolumeOptions
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00019EF4 File Offset: 0x000180F4
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00019EFC File Offset: 0x000180FC
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00019F05 File Offset: 0x00018105
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x00019F0D File Offset: 0x0001810D
		public Utf8String RoomName { get; set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00019F16 File Offset: 0x00018116
		// (set) Token: 0x060011CA RID: 4554 RVA: 0x00019F1E File Offset: 0x0001811E
		public float Volume { get; set; }
	}
}
