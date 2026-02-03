using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x0200043D RID: 1085
	public struct PromoteMemberOptions
	{
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x000299FA File Offset: 0x00027BFA
		// (set) Token: 0x06001C7E RID: 7294 RVA: 0x00029A02 File Offset: 0x00027C02
		public Utf8String LobbyId { get; set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x00029A0B File Offset: 0x00027C0B
		// (set) Token: 0x06001C80 RID: 7296 RVA: 0x00029A13 File Offset: 0x00027C13
		public ProductUserId LocalUserId { get; set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x00029A1C File Offset: 0x00027C1C
		// (set) Token: 0x06001C82 RID: 7298 RVA: 0x00029A24 File Offset: 0x00027C24
		public ProductUserId TargetUserId { get; set; }
	}
}
