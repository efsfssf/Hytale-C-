using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039B RID: 923
	public struct JoinLobbyByIdOptions
	{
		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0002448A File Offset: 0x0002268A
		// (set) Token: 0x060018CA RID: 6346 RVA: 0x00024492 File Offset: 0x00022692
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x0002449B File Offset: 0x0002269B
		// (set) Token: 0x060018CC RID: 6348 RVA: 0x000244A3 File Offset: 0x000226A3
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x000244AC File Offset: 0x000226AC
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x000244B4 File Offset: 0x000226B4
		public bool PresenceEnabled { get; set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x000244BD File Offset: 0x000226BD
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x000244C5 File Offset: 0x000226C5
		public LocalRTCOptions? LocalRTCOptions { get; set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x000244CE File Offset: 0x000226CE
		// (set) Token: 0x060018D2 RID: 6354 RVA: 0x000244D6 File Offset: 0x000226D6
		public bool CrossplayOptOut { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x000244DF File Offset: 0x000226DF
		// (set) Token: 0x060018D4 RID: 6356 RVA: 0x000244E7 File Offset: 0x000226E7
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType { get; set; }
	}
}
