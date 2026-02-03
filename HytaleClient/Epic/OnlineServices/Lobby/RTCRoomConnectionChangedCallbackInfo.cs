using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000447 RID: 1095
	public struct RTCRoomConnectionChangedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x00029FFE File Offset: 0x000281FE
		// (set) Token: 0x06001CBF RID: 7359 RVA: 0x0002A006 File Offset: 0x00028206
		public object ClientData { get; set; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0002A00F File Offset: 0x0002820F
		// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x0002A017 File Offset: 0x00028217
		public Utf8String LobbyId { get; set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0002A020 File Offset: 0x00028220
		// (set) Token: 0x06001CC3 RID: 7363 RVA: 0x0002A028 File Offset: 0x00028228
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0002A031 File Offset: 0x00028231
		// (set) Token: 0x06001CC5 RID: 7365 RVA: 0x0002A039 File Offset: 0x00028239
		public bool IsConnected { get; set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x0002A042 File Offset: 0x00028242
		// (set) Token: 0x06001CC7 RID: 7367 RVA: 0x0002A04A File Offset: 0x0002824A
		public Result DisconnectReason { get; set; }

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0002A054 File Offset: 0x00028254
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0002A070 File Offset: 0x00028270
		internal void Set(ref RTCRoomConnectionChangedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.LobbyId = other.LobbyId;
			this.LocalUserId = other.LocalUserId;
			this.IsConnected = other.IsConnected;
			this.DisconnectReason = other.DisconnectReason;
		}
	}
}
