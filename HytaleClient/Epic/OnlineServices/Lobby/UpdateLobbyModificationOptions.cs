using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000451 RID: 1105
	public struct UpdateLobbyModificationOptions
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x0002AAFA File Offset: 0x00028CFA
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x0002AB02 File Offset: 0x00028D02
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x0002AB0B File Offset: 0x00028D0B
		// (set) Token: 0x06001D2D RID: 7469 RVA: 0x0002AB13 File Offset: 0x00028D13
		public Utf8String LobbyId { get; set; }
	}
}
