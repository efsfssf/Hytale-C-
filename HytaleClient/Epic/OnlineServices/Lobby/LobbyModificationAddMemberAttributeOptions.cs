using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DE RID: 990
	public struct LobbyModificationAddMemberAttributeOptions
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0002888A File Offset: 0x00026A8A
		// (set) Token: 0x06001AF8 RID: 6904 RVA: 0x00028892 File Offset: 0x00026A92
		public AttributeData? Attribute { get; set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0002889B File Offset: 0x00026A9B
		// (set) Token: 0x06001AFA RID: 6906 RVA: 0x000288A3 File Offset: 0x00026AA3
		public LobbyAttributeVisibility Visibility { get; set; }
	}
}
