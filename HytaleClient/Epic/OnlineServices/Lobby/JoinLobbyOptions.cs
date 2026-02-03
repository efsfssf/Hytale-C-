using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200039F RID: 927
	public struct JoinLobbyOptions
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00024846 File Offset: 0x00022A46
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x0002484E File Offset: 0x00022A4E
		public LobbyDetails LobbyDetailsHandle { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00024857 File Offset: 0x00022A57
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x0002485F File Offset: 0x00022A5F
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00024868 File Offset: 0x00022A68
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x00024870 File Offset: 0x00022A70
		public bool PresenceEnabled { get; set; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00024879 File Offset: 0x00022A79
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x00024881 File Offset: 0x00022A81
		public LocalRTCOptions? LocalRTCOptions { get; set; }

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0002488A File Offset: 0x00022A8A
		// (set) Token: 0x060018FA RID: 6394 RVA: 0x00024892 File Offset: 0x00022A92
		public bool CrossplayOptOut { get; set; }

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0002489B File Offset: 0x00022A9B
		// (set) Token: 0x060018FC RID: 6396 RVA: 0x000248A3 File Offset: 0x00022AA3
		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType { get; set; }
	}
}
