using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000387 RID: 903
	public struct DestroyLobbyOptions
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x000238CE File Offset: 0x00021ACE
		// (set) Token: 0x0600184B RID: 6219 RVA: 0x000238D6 File Offset: 0x00021AD6
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x000238DF File Offset: 0x00021ADF
		// (set) Token: 0x0600184D RID: 6221 RVA: 0x000238E7 File Offset: 0x00021AE7
		public Utf8String LobbyId { get; set; }
	}
}
