using System;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000254 RID: 596
	public struct ParticipantUpdatedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00017A8D File Offset: 0x00015C8D
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x00017A95 File Offset: 0x00015C95
		public object ClientData { get; set; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00017A9E File Offset: 0x00015C9E
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x00017AA6 File Offset: 0x00015CA6
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00017AAF File Offset: 0x00015CAF
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00017AB7 File Offset: 0x00015CB7
		public Utf8String RoomName { get; set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00017AC0 File Offset: 0x00015CC0
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00017AC8 File Offset: 0x00015CC8
		public ProductUserId ParticipantId { get; set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00017AD1 File Offset: 0x00015CD1
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x00017AD9 File Offset: 0x00015CD9
		public bool Speaking { get; set; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00017AE2 File Offset: 0x00015CE2
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x00017AEA File Offset: 0x00015CEA
		public RTCAudioStatus AudioStatus { get; set; }

		// Token: 0x060010AA RID: 4266 RVA: 0x00017AF4 File Offset: 0x00015CF4
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00017B10 File Offset: 0x00015D10
		internal void Set(ref ParticipantUpdatedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Speaking = other.Speaking;
			this.AudioStatus = other.AudioStatus;
		}
	}
}
