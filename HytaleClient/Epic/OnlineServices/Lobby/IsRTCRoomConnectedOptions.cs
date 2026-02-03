using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000395 RID: 917
	public struct IsRTCRoomConnectedOptions
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x0002401C File Offset: 0x0002221C
		// (set) Token: 0x0600189B RID: 6299 RVA: 0x00024024 File Offset: 0x00022224
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x0002402D File Offset: 0x0002222D
		// (set) Token: 0x0600189D RID: 6301 RVA: 0x00024035 File Offset: 0x00022235
		public ProductUserId LocalUserId { get; set; }
	}
}
