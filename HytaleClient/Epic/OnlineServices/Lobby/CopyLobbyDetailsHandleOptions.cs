using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200037D RID: 893
	public struct CopyLobbyDetailsHandleOptions
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x00022F95 File Offset: 0x00021195
		// (set) Token: 0x060017E6 RID: 6118 RVA: 0x00022F9D File Offset: 0x0002119D
		public Utf8String LobbyId { get; set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00022FA6 File Offset: 0x000211A6
		// (set) Token: 0x060017E8 RID: 6120 RVA: 0x00022FAE File Offset: 0x000211AE
		public ProductUserId LocalUserId { get; set; }
	}
}
