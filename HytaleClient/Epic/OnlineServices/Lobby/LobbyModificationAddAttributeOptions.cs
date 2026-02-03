using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003DC RID: 988
	public struct LobbyModificationAddAttributeOptions
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x000287CE File Offset: 0x000269CE
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x000287D6 File Offset: 0x000269D6
		public AttributeData? Attribute { get; set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x000287DF File Offset: 0x000269DF
		// (set) Token: 0x06001AF1 RID: 6897 RVA: 0x000287E7 File Offset: 0x000269E7
		public LobbyAttributeVisibility Visibility { get; set; }
	}
}
