using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000389 RID: 905
	public struct GetConnectStringOptions
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001853 RID: 6227 RVA: 0x0002399A File Offset: 0x00021B9A
		// (set) Token: 0x06001854 RID: 6228 RVA: 0x000239A2 File Offset: 0x00021BA2
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x000239AB File Offset: 0x00021BAB
		// (set) Token: 0x06001856 RID: 6230 RVA: 0x000239B3 File Offset: 0x00021BB3
		public Utf8String LobbyId { get; set; }
	}
}
