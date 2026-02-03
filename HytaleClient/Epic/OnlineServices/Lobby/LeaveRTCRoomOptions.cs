using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003B1 RID: 945
	public struct LeaveRTCRoomOptions
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001986 RID: 6534 RVA: 0x00025662 File Offset: 0x00023862
		// (set) Token: 0x06001987 RID: 6535 RVA: 0x0002566A File Offset: 0x0002386A
		public Utf8String LobbyId { get; set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x00025673 File Offset: 0x00023873
		// (set) Token: 0x06001989 RID: 6537 RVA: 0x0002567B File Offset: 0x0002387B
		public ProductUserId LocalUserId { get; set; }
	}
}
