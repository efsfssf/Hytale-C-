using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200038F RID: 911
	public struct GetRTCRoomNameOptions
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x00023BA2 File Offset: 0x00021DA2
		// (set) Token: 0x0600186C RID: 6252 RVA: 0x00023BAA File Offset: 0x00021DAA
		public Utf8String LobbyId { get; set; }

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x00023BB3 File Offset: 0x00021DB3
		// (set) Token: 0x0600186E RID: 6254 RVA: 0x00023BBB File Offset: 0x00021DBB
		public ProductUserId LocalUserId { get; set; }
	}
}
