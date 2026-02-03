using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000272 RID: 626
	public struct UpdateParticipantVolumeOptions
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x0001960C File Offset: 0x0001780C
		// (set) Token: 0x0600116E RID: 4462 RVA: 0x00019614 File Offset: 0x00017814
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x0001961D File Offset: 0x0001781D
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x00019625 File Offset: 0x00017825
		public Utf8String RoomName { get; set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x0001962E File Offset: 0x0001782E
		// (set) Token: 0x06001172 RID: 4466 RVA: 0x00019636 File Offset: 0x00017836
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0001963F File Offset: 0x0001783F
		// (set) Token: 0x06001174 RID: 4468 RVA: 0x00019647 File Offset: 0x00017847
		public float Volume { get; set; }
	}
}
